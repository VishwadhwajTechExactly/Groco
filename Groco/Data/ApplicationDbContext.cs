using Groco.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Groco.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Fresh Grapes", Price = 50, Rating = 4, ImageUrl = ""},
                new Product { ProductId = 2, Name = "Fresh Guava", Price = 50, Rating = 4, ImageUrl = "" },
                new Product { ProductId = 3, Name = "Fresh Mangoes", Price = 50, Rating = 4, ImageUrl = "" }
                );
        }
    }
}
