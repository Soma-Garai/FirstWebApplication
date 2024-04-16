namespace FirstWebApplication.ViewModels
{
    public class OrderViewModel
    {
        public int UserId { get; set; } // User placing the order
        public List<OrderItemViewModel> OrderItems { get; set; } // List of items in the order
        public OrderDetails OrderDetails { get; set; }
        
        public OrderViewModel()
        {
            OrderItems = new List<OrderItemViewModel>();
        }
    }

    public class OrderItemViewModel
    {
        public int ProductId { get; set; } // Id of the product
        public int Quantity { get; set; } // Quantity of the product in the order
        public int? TotalPrice { get; set; }
    }

}
