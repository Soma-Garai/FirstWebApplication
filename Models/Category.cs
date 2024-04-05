using FirstWebApplication.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace FirstWebApplication.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public List<ProductViewModel>? Products { get; set; } // Navigation property

    }
}
