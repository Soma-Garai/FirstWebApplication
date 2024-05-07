using FirstWebApplication.Models;
using FirstWebApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace FirstWebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;     
            _hostEnvironment = hostEnvironment; 
        }
        
        [HttpGet]
        [Authorize(Policy = "ProductCreatePolicy")]
        public IActionResult AddProduct()
        {
            ViewBag.Categories = _context.tblCategories.ToList(); // Retrieve all categories for dropdown
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Products products)
        {
            if (ModelState.IsValid)
            {
                string serverFolder = "";

                if (products.product_image != null)
                {
                    string folder = "images/";
                    folder += Guid.NewGuid().ToString() + products.product_image.FileName;
                    products.product_ImagePath = "/" + folder;
                    serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folder);

                    await products.product_image.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                }
                ProductViewModel model = new ProductViewModel
                {
                    product_name = products.product_name,
                    product_description = products.product_description,
                    product_price = products.product_price,
                    product_ImagePath = products.product_ImagePath,
                    //category = _context.tblCategories.Find(products.CategoryId)
                    CategoryId = products.CategoryId
                };
                // Update the product with the selected category
                //products.category = _context.tblCategories.Find(products.CategoryId);

                // Add the new product to the database
                _context.tblProducts.Add(model);
                await _context.SaveChangesAsync();

                // Redirect to the Chocolate action with the newly created product's ID
                return RedirectToAction("Chocolates", "Home");
            }

            // If ModelState is not valid, return to the view with errors
            ViewBag.Categories = _context.tblCategories.ToList(); // Retrieve all categories for dropdown
            return View(products);
        }



        [HttpGet]
        [Authorize(Policy = "ProductEditPolicy")]
        public IActionResult EditProduct(int id)
        {
            var product = _context.tblProducts.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(int id, ProductViewModel product)
        {
            if (id != product.product_id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    return View(product);
                }

                return RedirectToAction("Chocolates","Home");
            }

            // If ModelState is not valid, return the same view with model errors
            return View(product);
        }

        [HttpGet]
        [Authorize(Policy = "ProductDeletePolicy")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.tblProducts.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProductConfirmed(int product_id)
        {
            var product = _context.tblProducts.Find(product_id);
            if (product == null)
            {
                return NotFound();
            }

            _context.tblProducts.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Chocolates", "Home");
        }


    }
}


