using FirstWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using System.Security.Claims;

namespace FirstWebApplication.Controllers
{
    //public class CartController : Controller
    //{
    //    //private readonly Cart _cart;
    //    private readonly AppDbContext _context;

    //    public CartController(AppDbContext context)
    //    {
    //        //_cart = cart;
    //        _context = context;
    //    }

    //    // GET: Cart
    //    public IActionResult Cart()
    //    {
    //        // You might not directly load cart items from the database,
    //        // but instead use the Cart model to store temporary items
    //        return View();
    //    }

    //    // GET: Cart/AddToCart/1
    //    public IActionResult AddToCart(int id)
    //    {
    //        var product = _context.tblProducts.Find(id);

    //        if (product != null)
    //        {
    //            CartItem cartItem = _context.Carts.FirstOrDefault(c => c.product_id == id);

    //            if (cartItem != null)
    //            {
    //                cartItem.product_Quantity++; // If item already in cart, increase quantity
    //            }
    //            else
    //            {
    //                cartItem = new CartItem
    //                {
    //                    product_id = product.product_id,
    //                    product_name = product.product_name,
    //                    product_price = product.product_price,
    //                    product_Quantity = 1
    //                };

    //                _context.Carts.Add(cartItem); // Add new item to cart
    //            }

    //            _context.SaveChanges(); // Save changes to database
    //        }

    //        return RedirectToAction("Index", "Product");
    //    }

    //    // GET: Cart/RemoveFromCart/1
    //    public IActionResult RemoveFromCart(int id)
    //    {
    //        var cartItem = _context.Carts.FirstOrDefault(c => c.product_id == id);

    //        if (cartItem != null)
    //        {
    //            _context.Carts.Remove(cartItem); // Remove item from cart
    //            _context.SaveChanges(); // Save changes to database
    //        }

    //        return RedirectToAction("Index");
    //    }
    //}
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly Cart _cart; // Inject Cart

        public CartController(AppDbContext context, Cart cart)
        {
            _context = context;
            _cart = cart;
        }

        // GET: Cart
        [HttpGet]
        public IActionResult Cart()
        {
            //var cartItems = _cart.ProductItems.Select(p => new CartItem { Products = p }).ToList();
            // You might want to pass cartItems to the view to display the cart
            return View(_cart.ProductItems);
        }

        //POST: Cart/AddToCart
       //[HttpPost]
       // //[ValidateAntiForgeryToken]
       // public IActionResult AddToCart(int product_id)
       // {
       //     var products = _context.tblProducts.FirstOrDefault(p => p.product_id == product_id);
       //     if (products != null)
       //     {
       //         var cart = new Cart(products, 1); // Assuming quantity 1 for now
       //         _cart.AddItem(cart);
       //     }
       //     return RedirectToAction("Cart");
       // }

        // POST: Cart/RemoveFromCart
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int productId)
        {
            _cart.RemoveItem(productId);
            return RedirectToAction("Index");
        }
    }

}
