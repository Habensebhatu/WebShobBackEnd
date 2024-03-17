using System;
using Data_layer.Context;
using Data_layer.Context.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<LoginEnitiyModel> Login{ get; set; }
        public DbSet<CustomerEntityModel> Customer { get; set; }
        public DbSet<CartEnityModel> Cart { get; set; }
        public DbSet<ProductImageEnityModel> ProductImage { get; set; }
        public DbSet<UserRegistrationEntityModel> UserRegistration { get; set; }
        public DbSet<WishlistEntityModel> WishlistEntityModel { get; set; }
        public DbSet<ProductWishlist> ProductWishlist { get; set; }

      

        public MyDbContext()
        {

        }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               optionsBuilder.UseSqlServer("Server=tcp:webshopserverf.database.windows.net,1433;Initial Catalog=Webshop;Persist Security Info=False;User ID=Filimon;Password=Icatt@06;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRegistrationEntityModel>().OwnsOne(u => u.Address);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Product>()
              .Property(p => p.Kilo)
              .HasColumnType("decimal(18,2)");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartEnityModel>()
              .Property(p => p.Price)
              .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CartEnityModel>()
             .Property(p => p.Kilo)
             .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetail>()
               .Property(p => p.AmountTotal)
               .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerEntityModel>()
        .HasKey(c => c.CustomerId);
        }
    }
}


