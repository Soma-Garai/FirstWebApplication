using FirstWebApplication.ViewModels;

namespace FirstWebApplication.Models
{
    public class CartItem
    {
        public ProductViewModel Product { get; set; } // The product in the cart
        public int Quantity { get; set; }    // Quantity of the product
    }

}

