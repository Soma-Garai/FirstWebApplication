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

        //[HttpPost]
        //public IActionResult AddOrder(OrderViewModel orderViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Get the logged-in user's ID
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //        // Create the Order entity
        //        var order = new Orders
        //        {
        //            UserId = userId,
        //            OrderDate = DateTime.Now,
        //            Status = OrderStatus.Shipped, // Set the status as needed
        //            //OrderDetails = orderViewModel.OrderDetails
        //        };

        //        _context.tblOrders.Add(order);
        //        _context.SaveChanges();

        //        // Now, for each item in the order, create OrderDetails
        //        foreach (var item in orderViewModel.OrderItems)
        //        {
        //            var product = _context.tblProducts.Find(item.ProductId);
        //            if (product == null)
        //            {
        //                // Handle invalid product scenario
        //                continue; // Skip this item and proceed to the next
        //            }

        //            var orderDetail = new OrderDetails
        //            {
        //                OrderId = order.OrderId, // Assign the newly created OrderId
        //                ProductId = product.product_id,
        //                ProductName = product.product_name,
        //                Quantity = item.Quantity,
        //                product_price = product.product_price
        //            };

        //            _context.tblOrderDetails.Add(orderDetail);
        //        }

        //        _context.SaveChanges();

        //        return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        //    }

        //    // If ModelState is not valid, return to the view with errors
        //    // You might want to handle errors in your ViewModel here
        //    return View(orderViewModel);
        //}

        

        [HttpPost]
        public async Task<IActionResult> Checkout(OrderViewModel model)
        {
            // Get the current logged-in user
            UserModel currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                // User not found, handle accordingly
                return RedirectToAction("Login", "User"); // Redirect to login page or handle as needed
            }

            // Now you can access the UserId
            string userId = currentUser.UserId;

            var order = new Orders
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = Orders.OrderStatus.Shipped,  // Set the status as needed
                OrderDetails = new List<OrderDetails>()
            };

            // Calculate TotalPrice for each OrderDetails item and sum them up
            int? TotalPrice = 0;
            foreach (var item in model.OrderItems)
            {
                var product = _context.tblProducts.Find(item.ProductId);
                if (product != null)
                {
                    var orderDetail = new OrderDetails
                    {
                        ProductId = product.product_id,
                        ProductName = product.product_name,
                        Quantity = item.Quantity,
                        product_price = product.product_price
                    };

                    // Calculate TotalPrice for this order detail
                    orderDetail.TotalPrice = orderDetail.Quantity * orderDetail.product_price;

                    // Add the order detail to the order
                    order.OrderDetails.Add(orderDetail);

                    // Add the total price of this order detail to the total price of the order
                    TotalPrice = TotalPrice + orderDetail.TotalPrice;
                }
            }

            // Set the total price of the order
            order.TotalPrice = TotalPrice;

            // Save the order to the database
            _context.tblOrders.Add(order);
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
        public IActionResult ViewOrder(int orderId)  //It provides a way for users to review the
                                                     //details of their order after they have placed it.
        {
            var order = _context.tblOrders
                .Include(o => o.OrderDetails) // Include OrderDetails for this order
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public IActionResult DeleteOrder(int orderId)
        {
            var order = _context.tblOrders
                .Include(o => o.OrderDetails) // Include OrderDetails for this order
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            // Remove OrderDetails associated with this order
            _context.tblOrderDetails.RemoveRange(order.OrderDetails);

            // Remove the Order itself
            _context.tblOrders.Remove(order);

            _context.SaveChanges();

            return RedirectToAction("OrdersList");
        }
    }
}
