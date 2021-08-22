using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UserTier.Models
{
    public class OrderModel
    {
        public List<ProductModel> ProductModel { get; set; }
        public List<CustomerModel> CustomerModel { get; set; }
        [Required(ErrorMessage = "Enter order ID.")]
        [Display(Name = "Order ID")]
        [Range(1, 999999, ErrorMessage = "Order ID need to be bweteen 1 and 999999")]
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Enter quantity.")]
        [Display(Name = "Quantity")]
        [Range(1, 9999999, ErrorMessage = "Quantity need to be between 1 an 9999999")]
        public int Quantity { get; set; }
        public float Total { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
    }
}