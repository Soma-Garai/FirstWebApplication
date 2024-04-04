using FirstWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FirstWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult About() //done
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
        public IActionResult Testimonial()//done
        {
            return View();
        }
        public IActionResult ContactUs()//done
        {
            return View();
        }
        
    }
}
