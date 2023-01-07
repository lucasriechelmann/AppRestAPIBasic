using AppRestAPIBasic.Business.Models;

namespace AppRestAPIBasic.Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetProductsBySupplierAsync(Guid supplierId);
        Task<List<Product>> GetProductsSuppliersAsync();
        Task<Product> GetProductSupplierAsync(Guid id);
    }
}
