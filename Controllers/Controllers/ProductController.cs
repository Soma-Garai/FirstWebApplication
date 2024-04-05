using FirstWebApplication.Models;
using FirstWebApplication.ViewModels;
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
        public IActionResult AddProduct()
        {
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
        public IActionResult EditProduct(int id, Products product)
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


//[HttpPost]
//public async Task<IActionResult> AddProduct(Products products)
//{
//    if (ModelState.IsValid)
//    {
//        string uniqueFileName = null;
//        if (products.product_image != null)
//        {
//            // Define the uploads folder path
//            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");

//            // Create a unique file name
//            uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(products.product_image.FileName);
//            products.product_ImagePath = uniqueFileName;
//            // Combine the folder path with the unique file name
//            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

//            // Copy the uploaded file to the specified path
//            using (var fileStream = new FileStream(filePath, FileMode.Create))
//            {
//                await products.product_image.CopyToAsync(fileStream);
//            }
//        }

//        // Create a new Products object with the data from the view model
//        ProductViewModel model = new ProductViewModel
//        {
//            product_name = products.product_name,
//            product_description = products.product_description,
//            product_price = products.product_price,
//            product_ImagePath = uniqueFileName
//        };

//        // Add the new product to the database
//        _context.tblProducts.Add(model);
//        await _context.SaveChangesAsync();

//        // Redirect to the Chocolate action with the newly created product's ID
//        return RedirectToAction("Chocolates", "Home");
//    }

//    // If ModelState is not valid, return to the view with errors
//    return View(products);
//}
//if (product.product_image != null && product.product_image.Length > 0)
//{
//    // Get the content root path (outside wwwroot)
//    string contentRootPath = _hostEnvironment.ContentRootPath;

//    // Create a unique file name to avoid conflicts
//    string fileName = Guid.NewGuid().ToString() + "_" + product.product_image.FileName;

//    // Combine the content root path with the Vendor/images folder
//    string uploadsFolder = Path.Combine(contentRootPath, "Vendor", "images");

//    // Combine the folder path with the unique file name to get the full path
//    string filePath = Path.Combine(uploadsFolder, fileName);

//    // Create the directory if it doesn't exist
//    if (!Directory.Exists(uploadsFolder))
//    {
//        Directory.CreateDirectory(uploadsFolder);
//    }

//    // Create a stream to copy the uploaded file
//    using (var stream = new FileStream(filePath, FileMode.Create))
//    {
//        await product.product_image.CopyToAsync(stream);
//    }

//    // Save the file path to the model for storage in the database
//    product.product_ImagePath = Path.Combine("Vendor", "images", fileName);
//}

//// Save the product to the database
//_context.tblProducts.Add(product);
//await _context.SaveChangesAsync();

//return RedirectToAction("Chocolates", "Home"); // Redirect to a success page
