namespace PhoneStore.Infrastructure.DataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
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
        public AppDbContext(DbContextOptions<AppDbContext> context) : base(context)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server =HIEU; database = PhoneStore;uid=sa;pwd=123;Encrypt=True;TrustServerCertificate=True");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountRole>().HasKey(sc => new { sc.AccountId, sc.RoleId });
            modelBuilder.Entity<AccountRole>().HasOne<Account>(sc => sc.Account)
            .WithMany(s => s.AccountRoles).HasForeignKey(sc => sc.AccountId);
            modelBuilder.Entity<AccountRole>().HasOne<Role>(sc => sc.Role)
            .WithMany(s => s.AccountRoles).HasForeignKey(sc => sc.RoleId);
            modelBuilder.Entity<Account>()
            .HasOne(a => a.Cart)
            .WithOne(c => c.Account)
            .HasForeignKey<Cart>(c => c.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Account)
                .WithOne(a => a.Cart)
                .HasForeignKey<Account>(a => a.CartId);
            modelBuilder.Entity<Review>().HasKey(sc => new { sc.AccountId, sc.ProductId });
            modelBuilder.Entity<Review>().HasOne<Account>(sc => sc.Account)
            .WithMany(s => s.AccountReview).HasForeignKey(sc => sc.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Review>().HasOne<Product>(sc => sc.Product)
            .WithMany(s => s.ProductReview).HasForeignKey(sc => sc.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProductVariants>()
            .HasOne(pv => pv.ProductImages)
            .WithOne(pi => pi.ProductVariants)
            .HasForeignKey<ProductImage>(pi => pi.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderItem>()
    .Property(o => o.PriceAtTime)
    .HasColumnType("decimal(18,2)");

        }
    }
}