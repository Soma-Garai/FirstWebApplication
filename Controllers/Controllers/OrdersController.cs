using FirstWebApplication.Extensions;
using FirstWebApplication.Models;
using FirstWebApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static FirstWebApplication.Models.Orders;

namespace FirstWebApplication.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        public OrdersController(AppDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Cart model)
        {
            var cart = HttpContext.Session.Get<Cart>("Cart") ?? new Cart();
            int i = cart.Items.Count;
            // Get the current logged-in user
            UserModel currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                // User not found, handle accordingly
                return RedirectToAction("Login", "User"); // Redirect to login page or handle as needed
            }

            // Now you can access the UserId
            string userId = currentUser.Id;

            var order = new Orders
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = Orders.OrderStatus.Shipped,  // Set the status as needed
                OrderDetails = new List<OrderDetails>()
            };
            // Save the order to the database
            _context.tblOrders.Add(order);
            _context.SaveChanges();

            // Calculate TotalPrice for each OrderDetails item and sum them up
            int? TotalPrice = 0;
            foreach (var item in cart.Items)
            {
                int quantity = item.Quantity;
                var productViewModel = item.Product;
                var orderDetail = new OrderDetails
                {                   
                    OrderId = order.OrderId,
                    
                    ProductId = productViewModel.product_id,
                    ProductName = productViewModel.product_name,
                    Quantity = quantity,
                    product_price = productViewModel.product_price
                };

                // Calculate TotalPrice for this order detail
                orderDetail.TotalPrice = orderDetail.Quantity * orderDetail.product_price;

                // Add the order detail to the database
                _context.tblOrderDetails.Add(orderDetail);
                _context.SaveChanges();
                
                // Add the total price of this order detail to the total price of the order
                TotalPrice = TotalPrice + orderDetail.TotalPrice;
            }

            // Set the total price of the order
            order.TotalPrice = TotalPrice;

            // Save the order to the database
            _context.SaveChanges();

            // Optionally, you can clear the session or any other cleanup
            HttpContext.Session.Remove("OrderDetails");

            // Optionally, you can redirect to a confirmation page with the order ID
            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

        public IActionResult OrderConfirmation(int orderId)
        {
            // Retrieve the order from the database based on orderId
            var order = _context.tblOrders
                              .Include(o => o.OrderDetails) // Include OrderDetails for this order
                              .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                // Handle case where order is not found
                return RedirectToAction("Index", "Home"); // Redirect to home or error page
            }

            // Pass the order to the view
            return View(order);
        }
        //public IActionResult ViewOrder(int orderId)  //It provides a way for users to review the
        //                                             //details of their order after they have placed it.
        //{
        //    var order = _context.tblOrders
        //        .Include(o => o.OrderDetails) // Include OrderDetails for this order
        //        .FirstOrDefault(o => o.OrderId == orderId);

        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}

        //[HttpPost]
        //public IActionResult DeleteOrder(int orderId)
        //{
        //    var order = _context.tblOrders
        //        .Include(o => o.OrderDetails) // Include OrderDetails for this order
        //        .FirstOrDefault(o => o.OrderId == orderId);

        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    // Remove OrderDetails associated with this order
        //    _context.tblOrderDetails.RemoveRange(order.OrderDetails);

        //    // Remove the Order itself
        //    _context.tblOrders.Remove(order);

        //    _context.SaveChanges();

        //    return RedirectToAction("OrdersList");
        //}
    }
}
