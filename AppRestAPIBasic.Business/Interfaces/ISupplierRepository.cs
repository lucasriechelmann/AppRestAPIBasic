using AppRestAPIBasic.Business.Models;

namespace AppRestAPIBasic.Business.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<Supplier> GetSupplierAddressAsync(Guid id);
        Task<Supplier> GetSupplierProductsAddressAsync(Guid id);
    }
}
