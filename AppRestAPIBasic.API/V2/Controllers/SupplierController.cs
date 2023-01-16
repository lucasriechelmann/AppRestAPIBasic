using AppRestAPIBasic.Api.ViewModels;
using AppRestAPIBasic.API.Controllers;
using AppRestAPIBasic.API.Extensions;
using AppRestAPIBasic.Business.Interfaces;
using AppRestAPIBasic.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppRestAPIBasic.API.V2.Controllers;

[Authorize]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/supplier")]
public class SupplierController : MainController
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly ISupplierService _supplierService;
    private readonly IAddressRepository _addressRepository;
    readonly IMapper _mapper;

    public SupplierController(ISupplierRepository supplierRepository,
        IMapper mapper,
        ISupplierService supplierService,
        INotifier notifier,
        IAddressRepository addressRepository,
        IUser appUser,
        ILogger<SupplierController> logger) : base(notifier, appUser, logger)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
        _supplierService = supplierService;
        _addressRepository = addressRepository;
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAsync());
        return Ok(suppliers);
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var supplier = await GetSupplierProductsAddress(id);

        if (supplier is null)
            return NotFound();

        return Ok(supplier);
    }
    [ClaimsAuthorize("Supplier", "Add")]
    [HttpPost]
    public async Task<IActionResult> Post(SupplierViewModel supplierViewModel)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        await _supplierService.AddAsync(_mapper.Map<Supplier>(supplierViewModel));

        return CustomResponse(supplierViewModel);
    }
    [ClaimsAuthorize("Supplier", "Edit")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, SupplierViewModel supplierViewModel)
    {
        if (id != supplierViewModel.Id)
        {
            NotifyError("The id in the route is different from the body");
            return CustomResponse();
        }

        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        await _supplierService.UpdateAsync(_mapper.Map<Supplier>(supplierViewModel));

        return CustomResponse(supplierViewModel);
    }
    [ClaimsAuthorize("Supplier", "Delete")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var supplier = await GetSupplierAddress(id);

        if (supplier is null)
            return NotFound();

        await _supplierService.DeleteAsync(id);
        return CustomResponse();
    }

    [HttpGet("get-address/{id:guid}")]
    public async Task<IActionResult> GetAddressById(Guid id)
    {
        return CustomResponse(_mapper.Map<AddressViewModel>(await _addressRepository.GetAddressBySupplierAsync(id)));
    }
    [ClaimsAuthorize("Supplier", "Edit")]
    [HttpPut("update-address/{id:guid}")]
    public async Task<IActionResult> UpdateAddress(Guid id, AddressViewModel addressViewModel)
    {
        if (id != addressViewModel.Id)
        {
            NotifyError("The id in the route is different from the body");
            return CustomResponse();
        }

        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        await _supplierService.UpdateAddressAsync(_mapper.Map<Address>(addressViewModel));

        return CustomResponse(addressViewModel);
    }
    async Task<SupplierViewModel> GetSupplierProductsAddress(Guid id)
    {
        return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierProductsAddressAsync(id));
    }
    async Task<SupplierViewModel> GetSupplierAddress(Guid id)
    {
        return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddressAsync(id));
    }
}