using AppRestAPIBasic.Api.ViewModels;
using AppRestAPIBasic.API.Controllers;
using AppRestAPIBasic.API.Extensions;
using AppRestAPIBasic.Business.Interfaces;
using AppRestAPIBasic.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppRestAPIBasic.API.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/product")]
    public class ProductsController : MainController
    {
        private readonly IProductRepository _productRepository;
        readonly IProductService _productService;
        readonly IMapper _mapper;
        public ProductsController(INotifier notifier,
            IProductRepository productRepository,
            IProductService productService,
            IMapper mapper,
            IUser appUser,
            ILogger<ProductsController> logger) : base(notifier, appUser, logger)
        {
            _productRepository = productRepository;
            _productService = productService;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return CustomResponse(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsSuppliersAsync()));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel is null)
                return NotFound();

            return CustomResponse(productViewModel);
        }
        [ClaimsAuthorize("Product", "Add")]
        [HttpPost]
        public async Task<IActionResult> Post(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            productViewModel.Image = $"{Guid.NewGuid()}_{productViewModel.Image}";

            if (!UploadImage(productViewModel.ImageUpload, productViewModel.Image))
                return CustomResponse();

            await _productService.AddAsync(_mapper.Map<Product>(productViewModel));

            return CustomResponse();
        }
        [ClaimsAuthorize("Product", "Add")]
        [HttpPost("streaming")]
        public async Task<IActionResult> PostWithStreaming([ModelBinder(BinderType = typeof(ProductModelBinder))] ProductImageViewModel productViewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var imgPrefix = $"{Guid.NewGuid()}_";

            if (!await UploadImageStreaming(productViewModel.ImageUpload, imgPrefix))
                return CustomResponse();


            productViewModel.Image = $"{imgPrefix}{productViewModel.Image}";

            await _productService.AddAsync(_mapper.Map<Product>(productViewModel));

            return CustomResponse();
        }
        [ClaimsAuthorize("Product", "Add")]
        [RequestSizeLimit(41943040)] // 40mb in bytes
        [HttpPost("image")]
        public async Task<IActionResult> Post(IFormFile image)
        {
            return await Task.FromResult(Ok(image));
        }
        [ClaimsAuthorize("Product", "Edit")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                NotifyError("The route id is different form the body id!");
                return CustomResponse();
            }

            var productUpdate = await GetProduct(id);

            if (string.IsNullOrEmpty(productViewModel.Image))
                productViewModel.Image = productUpdate.Image;

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (productViewModel.ImageUpload != null)
            {
                var imageName = Guid.NewGuid() + "_" + productViewModel.Image;
                if (!UploadImage(productViewModel.ImageUpload, imageName))
                {
                    return CustomResponse(ModelState);
                }

                productUpdate.Image = imageName;
            }

            productUpdate.SupplierId = productViewModel.SupplierId;
            productUpdate.Name = productViewModel.Name;
            productUpdate.Description = productViewModel.Description;
            productUpdate.Value = productViewModel.Value;
            productUpdate.Active = productViewModel.Active;

            await _productService.UpdateAsync(_mapper.Map<Product>(productUpdate));

            return CustomResponse(productViewModel);
        }
        [ClaimsAuthorize("Product", "Delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel is null)
                return NotFound();

            await _productService.DeleteAsync(id);

            return CustomResponse(productViewModel);
        }

        bool UploadImage(string file, string imgName)
        {
            if (string.IsNullOrEmpty(file))
            {
                NotifyError("Image is required.");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(file);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgName);

            if (System.IO.File.Exists(filePath))
            {
                NotifyError("Image already uploaded.");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }

        async Task<bool> UploadImageStreaming(IFormFile file, string imgPrefix)
        {
            if (file is null || file.Length <= 0)
            {
                NotifyError("Image is required.");
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", $"{imgPrefix}{file.FileName}");

            if (System.IO.File.Exists(filePath))
            {
                NotifyError("Image already uploaded.");
                return false;
            }

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return true;
        }

        async Task<ProductViewModel> GetProduct(Guid id) =>
            _mapper.Map<ProductViewModel>(await _productRepository.GetProductSupplierAsync(id));
    }
}
