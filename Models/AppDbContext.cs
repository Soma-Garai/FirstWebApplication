
using FirstWebApplication.Models;
using FirstWebApplication.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<UserModel>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    //public DbSet<UserModel> tblUsers { get; set; }
    public DbSet<ProductViewModel> tblProducts { get; set; }
    public DbSet<Category> tblCategories { get; set; }
    //public DbSet<CartItem> Carts { get; set; } // DbSet for CartItems

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed categories
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, CategoryName = "Dark Chocolate" },
            new Category { CategoryId = 2, CategoryName = "Milk Chocolate" },
            new Category { CategoryId = 3, CategoryName = "White Chocolate" },
            new Category { CategoryId = 4, CategoryName = "Candy" }
            // Add more categories as needed
        );
    }

}
