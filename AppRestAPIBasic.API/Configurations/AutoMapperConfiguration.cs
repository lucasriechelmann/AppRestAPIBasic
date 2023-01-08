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
            CreateMap<ProductViewModel, Product>();

            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name));
        }
    }
}
