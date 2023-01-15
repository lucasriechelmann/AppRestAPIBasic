using AppRestAPIBasic.API.Extensions;
using AppRestAPIBasic.Business.Interfaces;
using AppRestAPIBasic.Business.Notifications;
using AppRestAPIBasic.Business.Services;
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
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<INotifier, Notifier>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            return services;
        }
    }
}
