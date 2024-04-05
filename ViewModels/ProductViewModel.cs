using FirstWebApplication.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstWebApplication.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public int product_id { get; set; }
        public string? product_name { get; set; }
        public string? product_description { get; set; }
        public int? product_price { get; set; }
        public string? product_ImagePath { get; set; }
        //[NotMapped]
        //public Cart Cart { get; set; }


        public int CategoryId { get; set; }      // Foreign key
        public Category? category { get; set; }  // Navigation property
    }
}
