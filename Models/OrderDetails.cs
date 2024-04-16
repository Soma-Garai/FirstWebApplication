using FirstWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstWebApplication.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderItemId { get; set; } // Primary Key 

            //[Foreign Key to tblOrders]
        public int ProductId { get; set; } // [Foreign Key to tblProducts]
        public string? ProductName { get; set; }
        public int? Quantity { get; set; }
        public int? product_price { get; set; }
        public int? TotalPrice  { get; set; }

        [ForeignKey("Orders")]
        public int OrderId { get; set; }
        // Navigation property
        public Orders? Orders { get; set; }
        //public Products? Products { get; set; }
        
        
    }
}
