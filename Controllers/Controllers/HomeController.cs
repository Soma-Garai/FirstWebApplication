using FirstWebApplication.Models;
using FirstWebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace FirstWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;
        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            List<Category> categories = _appDbContext.tblCategories.ToList();

            // Pass categories to the view
            ViewBag.Categories = categories;
            return View();
        }
        
        public IActionResult About() 
        {
            return View();
        }
        public IActionResult Chocolates(int? categoryId)
        {
            // Retrieve categories from the database
            List<Category> categories = _appDbContext.tblCategories.ToList();

            // Pass categories to the view
            ViewBag.Categories = categories;

            // Retrieve products from the database along with categories
            var productsWithCategories = _appDbContext.tblProducts
                .Include(p => p.category) // Include category information
                .Select(p => new ProductWithCategoryViewModel
                {
                    product_id = p.product_id,
                    product_name = p.product_name,
                    product_description = p.product_description,
                    product_price = p.product_price,
                    product_ImagePath = p.product_ImagePath,
                    CategoryId = p.CategoryId,
                    CategoryName = p.category.CategoryName // Assuming there's a property for CategoryName in Category entity
                })
                .ToList();
            // Filter products based on categoryId if provided
            if (categoryId.HasValue)
            {
                productsWithCategories = productsWithCategories.Where(p => p.CategoryId == categoryId.Value).ToList();
            }
            ViewBag.ProductsWithCategories = productsWithCategories;
            return View();
        }


        public IActionResult Testimonial()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        
    }
}

//public IActionResult Chocolates()
//{
//    // Retrieve products from the database
//    List<ProductViewModel> allProducts = _appDbContext.tblProducts.ToList();

//    // Map ProductViewModel to Products
//    var products = allProducts.Select(p => new Products
//    {
//        product_id = p.product_id,
//        product_name = p.product_name,
//        product_description = p.product_description,
//        product_price = p.product_price,
//        product_ImagePath = p.product_ImagePath,
//        CategoryId = p.CategoryId,
//        CategoryName = p.category.CategoryName
//    }).ToList();
//    ViewBag.ProductsWithCategories = products;
//    return View();
//}
