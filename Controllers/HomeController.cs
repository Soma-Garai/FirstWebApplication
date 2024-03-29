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
        public IActionResult Chocolates()//done
        {
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
