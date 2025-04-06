

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace PhoneStore.Api
{
    public static class RegisterServices
    {
        public static void RegisterServicesMiddleware(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString ?? throw new Exception("Sai đường dẫn kết nối kiểm tra lại appsetting"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAccountServices, AccountServices>();
            services.AddTransient<IBrandServices, BrandServices>();
            services.AddTransient<ICapacityServices, CapacityServices>();
            services.AddTransient<ICartServices, CartServices>();
            services.AddTransient<IOrderServices, OrderServices>();
            services.AddTransient<IOrderItemServices, OrderItemServices>();
            services.AddTransient<IProductColorServices, ProductColorServices>();
            services.AddTransient<IProductImageServices, ProductImageServices>();
            services.AddTransient<IProductServices, ProductServices>();
            services.AddTransient<IProductVariantsServices, ProductVariantsServices>();
            services.AddTransient<IRefreshTokenServices>(servicesProvider =>
            {
                var unitOfWork = servicesProvider.GetRequiredService<IUnitOfWork>();
                return new RefreshTokenServices(unitOfWork, configuration);
            });
            services.AddTransient<IReviewServices, ReviewServices>();
            services.AddTransient<IRoleServices, RoleServices>();
            services.AddAutoMapper(typeof(ClassMapping));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? throw new Exception("Key not found")))
                };
            });
        }
    }
}
