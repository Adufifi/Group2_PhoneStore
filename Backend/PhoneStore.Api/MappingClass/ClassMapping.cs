

using PhoneStore.Application.Dto;
using PhoneStore.Domain.Models;

namespace PhoneStore.Api.MappingClass
{
    public class ClassMapping : Profile
    {
        public ClassMapping()
        {
            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}
