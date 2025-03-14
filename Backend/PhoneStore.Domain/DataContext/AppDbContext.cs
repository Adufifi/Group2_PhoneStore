using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Domain.Models;

namespace PhoneStore.Domain.DataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Capacity> Capacities { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariants> ProductVariants { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
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
            .HasForeignKey<Cart>(c => c.AccountId);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Account)
                .WithOne(a => a.Cart)
                .HasForeignKey<Account>(a => a.CartId);
        }
    }
}