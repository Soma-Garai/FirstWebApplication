using FirstWebApplication.ViewModels;

namespace FirstWebApplication.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; private set; } = new List<CartItem>();

        public void AddItem(ProductViewModel product, int quantity)
        {
            var existingItem = Items.FirstOrDefault(item => item.Product.product_id == product.product_id);

            if (existingItem != null)
            {
                // Update quantity if the item already exists in cart
                existingItem.Quantity += quantity;
            }
            else
            {
                // Add a new item to the cart
                Items.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
        }

        // Other methods for removing items, calculating total price, etc.
    }

}
