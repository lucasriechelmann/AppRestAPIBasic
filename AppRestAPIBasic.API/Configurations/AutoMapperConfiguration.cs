using AppRestAPIBasic.Api.ViewModels;
using AppRestAPIBasic.Business.Models;
using AutoMapper;

namespace AppRestAPIBasic.API.Configurations
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Supplier, SupplierViewModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
