using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using FirstWebApplication.Models;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("FirstWebApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'FirstWebApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FirstWebApplicationDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    options.LoginPath = "/User/Login"; // Customize your login path
    options.AccessDeniedPath = "/User/AccessDenied"; // Customize your access denied path
    options.SlidingExpiration = true;
});
builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();
builder.Services.AddScoped<Products>();
//builder.Services.AddScoped<Cart>();
builder.Services.AddScoped<Category>();
// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session support
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust as needed
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
app.UseSession(); // Use session middleware
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
