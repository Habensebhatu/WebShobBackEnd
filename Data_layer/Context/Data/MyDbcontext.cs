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
               optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Webshop;User Id=sa;Password=Admin@123;TrustServerCertificate=True;");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartEnityModel>()
              .Property(p => p.Price)
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


