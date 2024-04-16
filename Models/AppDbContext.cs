
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
    public DbSet<Orders> tblOrders { get; set; }
    public DbSet<OrderDetails> tblOrderDetails { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //// Configure the relationship
        //modelBuilder.Entity<OrderDetails>()
        //    .HasOne(od => od.Orders)            // OrderDetails has one Order
        //    .WithMany(o => o.OrderDetails)   // Order has many OrderDetails
        //    .HasForeignKey(od => od.OrderId); // Foreign key is OrderId

        modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });
        modelBuilder.Entity<IdentityUserRole<string>>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
        });
        modelBuilder.Entity<IdentityUserToken<string>>(userToken =>
        {
            userToken.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
        });

        // Seed categories
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, CategoryName = "Dark Chocolate" },
            new Category { CategoryId = 2, CategoryName = "Milk Chocolate" },
            new Category { CategoryId = 3, CategoryName = "White Chocolate" },
            new Category { CategoryId = 4, CategoryName = "Candy" }
            

        );
    }

}
