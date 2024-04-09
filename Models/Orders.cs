using System.ComponentModel.DataAnnotations;

namespace FirstWebApplication.Models
{
    public class Orders
    {
        [Key]
        public int OrderId{ get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }  // Added OrderStatus property
        public int? TotalPrice { get; set; }  // Total price of the entire order
        public enum OrderStatus 
        {
            Shipped,
            PartiallyShipped,
            Cancelled
        }

        // Navigation property for order details
        public List<OrderDetails> OrderDetails { get; set; }
        

    }


}
