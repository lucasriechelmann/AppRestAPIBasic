using AppRestAPIBasic.Business.Interfaces;
using AppRestAPIBasic.Data.Context;
using AppRestAPIBasic.Data.Repositories;

namespace AppRestAPIBasic.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MyDbContext>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
