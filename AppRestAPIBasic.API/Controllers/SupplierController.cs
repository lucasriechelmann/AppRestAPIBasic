using AppRestAPIBasic.Api.ViewModels;
using AppRestAPIBasic.Business.Interfaces;
using AppRestAPIBasic.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AppRestAPIBasic.API.Controllers;

[Route("api/[controller]")]
public class SupplierController : MainController
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly ISupplierService _supplierService;
    readonly IMapper _mapper;

    public SupplierController(ISupplierRepository supplierRepository, IMapper mapper, ISupplierService supplierService)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
        _supplierService = supplierService;
    }
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
    [HttpPost]
    public async Task<IActionResult> Post(SupplierViewModel supplierViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var supplier = _mapper.Map<Supplier>(supplierViewModel);
        await _supplierService.AddAsync(supplier);


        return Ok(supplier);
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, SupplierViewModel supplierViewModel)
    {
        if(id != supplierViewModel.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest();

        var supplier = _mapper.Map<Supplier>(supplierViewModel);
        await _supplierService.UpdateAsync(supplier);


        return Ok(supplier);
    }

    [HttpDelete("{id:guid")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var supplier = await GetSupplierAddress(id);

        if (supplier is null)
            return NotFound();

        await _supplierService.DeleteAsync(id);
        return Ok(supplier);
    }
    public async Task<SupplierViewModel> GetSupplierProductsAddress(Guid id)
    {
        return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierProductsAddressAsync(id));
    }
    public async Task<SupplierViewModel> GetSupplierAddress(Guid id)
    {
        return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddressAsync(id));
    }
}