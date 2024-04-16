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

        public void UpdateQuantity(Product product, int quantity)
        {
            var existingItem = Items.FirstOrDefault(item => item.ProductId == product.product_id);
            if (existingItem != null)
            {
                existingItem.Quantity = quantity;
            }
        }

        //public void RemoveItem(Product product)
        //{
        //    var itemToRemove = Items.FirstOrDefault(item => item.ProductId == product.product_id);
        //    if (itemToRemove != null)
        //    {
        //        Items.Remove(itemToRemove);
        //    }
        //}

        //public void Clear()
        //{
        //    Items.Clear();
        //}
    }

}
