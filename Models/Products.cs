using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstWebApplication.Models
{
    public class Products
    {
        public int product_id { get; set; }
        public string? product_name { get; set;}
        public string? product_description { get; set;}
        public int? product_price { get; set; }
        //public int product_Quantity { get; set; }

        

        //This property is for the form file upload, so it's not mapped to the database
        [NotMapped]
        public IFormFile? product_image { get; set; }
        public string? product_ImagePath { get; set; }
    }
}
