using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Yum.Data.Models;

namespace Yum.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartLineItem> CartLineItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Appetizer" },
                new Category { Id = 2, Name = "Entree" },
                new Category { Id = 3, Name = "Dessert" }
            );

            builder.Entity<OrderStatus>().HasData(
                new OrderStatus { Id = 1, Name = "Pending" },
                new OrderStatus { Id = 2, Name = "Ready" },
                new OrderStatus { Id = 3, Name = "Completed" },
                new OrderStatus { Id = 4, Name = "Cancelled" }
            );
        }
    }
}
