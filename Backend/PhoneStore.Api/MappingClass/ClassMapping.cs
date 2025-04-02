

namespace PhoneStore.Api.MappingClass
{
    public class ClassMapping : Profile
    {
        public ClassMapping()
        {
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
                // .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.ProductVariants))
                ;
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Brand, opt => opt.Ignore());
            CreateMap<Account, AccountResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Img, opt => opt.MapFrom(src => src.Img));
            CreateMap<Account, RegisterAccount>()
                .ForMember(desc => desc.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(desc => desc.Password, opt => opt.MapFrom(src => HashPassword(src.PassWord)))
                .ForMember(desc => desc.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<RegisterAccount, Account>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.PassWord, opt => opt.MapFrom(src => HasBCrypt(src.Password)));
            // CreateMap<ProductVariants, ProductVariantsDto>()
            // .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : null))
            // // .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Product != null ? src.Product.BrandName : null))
            // ;
            // CreateMap<ProductVariantsDto, ProductVariants>();
        }
        private static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            var salt = GenerateSalt();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hashBytes) + ":" + salt;
            }
        }

        private static string GenerateSalt(int size = 16)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[size];
                rng.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
        }
        private static string HasBCrypt(string pass)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(pass);
        }
    }
}
