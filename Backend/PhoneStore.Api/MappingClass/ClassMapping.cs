using PhoneStore.Domain.ModelResponse;
using System.Security.Cryptography;
using System.Text;

namespace PhoneStore.Api.MappingClass
{
    public class ClassMapping : Profile
    {
        public ClassMapping()
        {
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<AccountResponse, Account>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Img, opt => opt.MapFrom(src => src.Img));
            CreateMap<Account, RegisterAccount>()
                .ForMember(desc => desc.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(desc => desc.Password, opt => opt.MapFrom(src => ComputeHmacSha256(src.PassWord)))
                .ForMember(desc => desc.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<RegisterAccount, Account>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.PassWord, opt => opt.MapFrom(src => ComputeHmacSha256(src.Password)));

        }
        public static string ComputeHmacSha256(string data)
        {
            var encoding = new UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes("ItrvNYKjxqoLk2rX58UN39OQwitnytor");
            byte[] dataBytes = encoding.GetBytes(data);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hash = hmacsha256.ComputeHash(dataBytes);
                return Convert.ToBase64String(hash)
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .TrimEnd('=');
            }
        }
    }
}
