using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuickServe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Infrastructure
{
    public partial class QuickServeContext : DbContext
    {
        public QuickServeContext() { }
        public QuickServeContext(DbContextOptions options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<ProductIngredient> IngredientProducts { get; set; }
        public DbSet<IngredientType> IngredientTypes { get; set; }
        public DbSet<Nutrition> Nutritions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTemplate> ProductTemplates { get; set; }
        public DbSet<ProductTemplateIngredient> ProductTemplateIngredients { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AccessTokenBlacklist> AccessTokenBlacklists { get; set; }  
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyDB"));*/
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductIngredient>()
                .HasKey(pt => new { pt.ProductId, pt.IngredientId });

            modelBuilder.Entity<ProductTemplateIngredient>()
                .HasKey(pt => new { pt.ProductTemplateId, pt.IngredientId });
            modelBuilder.Entity<OrderProduct>()
                .HasKey(pt => new { pt.OrderId, pt.ProductId });

            modelBuilder.Entity<OrderProduct>()
                   .HasOne(op => op.Order)
                   .WithMany(o => o.OrderProducts)
                   .HasForeignKey(op => op.OrderId)
                   .OnDelete(DeleteBehavior.Cascade); // Xóa đơn hàng liên quan khi xóa OrderProduct

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.NoAction); // Không làm gì khi xóa sản phẩm liên quan

            base.OnModelCreating(modelBuilder);
        }
    }
}
