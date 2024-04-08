using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using FirstWebApplication.Models;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("FirstWebApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'FirstWebApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FirstWebApplicationDbContext>();
builder.Services.AddIdentity<UserModel, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();
builder.Services.AddScoped<UserManager<UserModel>>();
builder.Services.AddScoped<SignInManager<UserModel>>();
builder.Services.AddScoped<Products>();
//builder.Services.AddScoped<Cart>();
builder.Services.AddScoped<Category>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
