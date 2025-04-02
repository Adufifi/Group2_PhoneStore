namespace PhoneStore.Infrastructure.DataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariants> ProductVariants { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Capacity> Capacities { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> context) : base(context)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server =.; database = PhoneStore;uid=sa;pwd=12345678;Encrypt=True;TrustServerCertificate=True",
                b => b.MigrationsAssembly("PhoneStore.Infrastructure"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>().HasKey(sc => new { sc.AccountId, sc.ProductId });
            modelBuilder.Entity<Review>().HasOne<Account>(sc => sc.Account)
            .WithMany(s => s.AccountReview).HasForeignKey(sc => sc.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>().HasOne<Product>(sc => sc.Product)
            .WithMany(s => s.ProductReview).HasForeignKey(sc => sc.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProductVariants>()
            .HasOne(pv => pv.ProductImages)
            .WithOne(pi => pi.ProductVariants)
            .HasForeignKey<ProductImage>(pi => pi.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderItem>()
            .Property(o => o.PriceAtTime)
            .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Account>()
            .HasMany(a => a.RefreshTokens)
            .WithOne(r => r.Account)
            .HasForeignKey(r => r.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}