

using PhoneStore.Application.Dto;
using PhoneStore.Domain.Models;

namespace PhoneStore.Api.MappingClass
{
    public class ClassMapping : Profile
    {
        public ClassMapping()
        {
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
