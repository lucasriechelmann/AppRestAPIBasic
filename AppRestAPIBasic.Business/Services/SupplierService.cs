using AppRestAPIBasic.Business.Interfaces;
using AppRestAPIBasic.Business.Models;
using AppRestAPIBasic.Business.Models.Validations;

namespace AppRestAPIBasic.Business.Services
{
    public class SupplierService : BaseService, ISupplierService
    {
        readonly ISupplierRepository _supplierRepository;
        readonly IAddressRepository _addressRepository;

        public SupplierService(ISupplierRepository supplierRepository, IAddressRepository addressRepository, INotifier notifier) : base(notifier) 
        {
            _supplierRepository = supplierRepository;
            _addressRepository = addressRepository;
        }

        public async Task AddAsync(Supplier supplier)
        {
            if (!IsEntityValid(new SupplierValidation(), supplier) || !IsEntityValid(new AddressValidation(), supplier.Address))
                return;

            if((await _supplierRepository.GetAsync(x => x.Document == supplier.Document)).Any())
            {
                Notify("There is a supplier with the same document registered.");
                return;
            }

            await _supplierRepository.AddAsync(supplier);
        }

        public async Task DeleteAsync(Guid id)
        {
            if((await _supplierRepository.GetSupplierProductsAddressAsync(id)).Products.Any())
            {
                Notify("There supplier has products.");
                return;
            }

            await _supplierRepository.DeleteAsync(id);
        }

        public void Dispose()
        {
            _addressRepository?.Dispose();
            _supplierRepository?.Dispose();
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            if (!IsEntityValid(new SupplierValidation(), supplier))
                return;

            if ((await _supplierRepository.GetAsync(x => x.Document == supplier.Document && x.Id != supplier.Id)).Any())
            {
                Notify("There is a supplier with the same document registered.");
                return;
            }

            await _supplierRepository.UpdateAsync(supplier);
        }

        public async Task UpdateAddressAsync(Address address)
        {
            if (!IsEntityValid(new AddressValidation(), address))
                return;

            await _addressRepository.UpdateAsync(address);
        }
    }
}
