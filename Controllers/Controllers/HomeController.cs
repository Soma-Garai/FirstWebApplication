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
            return View();
        }
        
        public IActionResult About() 
        {
            return View();
        }
        public IActionResult Chocolates()
        {
            // Retrieve products from the database
            List<ProductViewModel> allProducts = _appDbContext.tblProducts.ToList();

            // Map ProductViewModel to Products
            var products = allProducts.Select(p => new Products
            {
                product_id = p.product_id,
                product_name = p.product_name,
                product_description = p.product_description,
                product_price = p.product_price,
                product_ImagePath = p.product_ImagePath
            }).ToList();
            ViewBag.Products = products;
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
