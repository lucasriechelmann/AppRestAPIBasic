using AppRestAPIBasic.Business.Interfaces;
using AppRestAPIBasic.Business.Models;
using AppRestAPIBasic.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRestAPIBasic.Data.Repositories
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(MyDbContext db) : base(db)
        {
        }

        public async Task<Address> GetAddressBySupplierAsync(Guid supplierId)
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SupplierId == supplierId);
        }
    }
}
