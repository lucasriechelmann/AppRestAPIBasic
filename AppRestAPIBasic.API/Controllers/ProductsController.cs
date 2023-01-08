using AppRestAPIBasic.Api.ViewModels;
using AppRestAPIBasic.Business.Interfaces;
using AppRestAPIBasic.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AppRestAPIBasic.API.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    public class ProductsController : MainController
    {
        private readonly IProductRepository _productRepository;
        readonly IProductService _productService;
        readonly IMapper _mapper;
        public ProductsController(INotifier notifier, 
            IProductRepository productRepository, 
            IProductService productService, 
            IMapper mapper) : base(notifier)
        {
            _productRepository = productRepository;
            _productService = productService;
            _mapper = mapper;
        }

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
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel is null)
                return NotFound();

            await _productService.DeleteAsync(id);

            return CustomResponse(productViewModel);
        }

        private bool UploadImage(string file, string imgName)
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

        private async Task<ProductViewModel> GetProduct(Guid id) =>
            _mapper.Map<ProductViewModel>(await _productRepository.GetProductSupplierAsync(id));
    }
}
