namespace PhoneStore.Api
{
    public static class RegisterServices
    {
        public static void RegisterServicesMiddleware(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Sai đường dẫn kết nối kiểm tra lại appsetting"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAccountRoleServices, AccountRoleServices>();
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
            services.AddAutoMapper(typeof(Program));
        }
    }
}
