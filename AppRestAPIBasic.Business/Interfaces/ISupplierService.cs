using AppRestAPIBasic.Business.Models;

namespace AppRestAPIBasic.Business.Interfaces
{
    public interface ISupplierService : IDisposable
    {
        Task AddAsync(Supplier supplier);        
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(Guid id);
        Task UpdateAddressAsync(Address address);
    }
}
