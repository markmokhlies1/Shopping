using API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class DataContext : IdentityDbContext<User, Role, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Option> Option { get; set; }
        public DbSet<Image> Images4 { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<WithdrawRequest> WithdrawRequests { get; set; }
        public DbSet<ProductOptionCart> ProductOptionCart { get; set; }
        public DbSet<UserBill> UserBills { get; set; }
        public DbSet<ShippingCompany> shippingCompanies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>()
              .HasOne<User>(s => s.Supplier)
              .WithMany(ta => ta.Products)
              .HasForeignKey(u => u.SupplierId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}