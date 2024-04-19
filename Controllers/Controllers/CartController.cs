using FirstWebApplication.Extensions;
using FirstWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using System.Security.Claims;

namespace FirstWebApplication.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppDbContext _context;

        public CartController(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _context = context;
        }

        public IActionResult AddToCart(int productId, int quantity)
        {
            var user = _contextAccessor.HttpContext.User;
            // Check if the user is authenticated
            if (!user.Identity.IsAuthenticated)
            {
                // Redirect the user to the login page
                return RedirectToAction("Login", "User");
            }
            else
            {
            // Find the product in the database
            var product = _context.tblProducts.Find(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Create a cart or get the existing cart from session
            var cart = HttpContext.Session.Get<Cart>("Cart") ?? new Cart();

            // Add the product to the cart with the specified quantity
            cart.AddItem(product, quantity);

            // Save the updated cart back to session
            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("ViewCart"); // Redirect to view cart page

            }
        }

        public IActionResult ViewCart()
        {
            var cart = HttpContext.Session.Get<Cart>("Cart") ?? new Cart();

            return View(cart);
        }
        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var product = _context.tblProducts.Find(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.Get<Cart>("Cart") ?? new Cart();
            cart.UpdateQuantity(product, quantity);

            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("ViewCart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var product = _context.tblProducts.Find(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.Get<Cart>("Cart") ?? new Cart();
            cart.RemoveItem(product);

            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("ViewCart");
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("ViewCart");
        }

    }
}
