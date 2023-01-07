using AppRestAPIBasic.Business.Models;

namespace AppRestAPIBasic.Business.Interfaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address> GetAddressBySupplierAsync(Guid supplierId);
    }
}
