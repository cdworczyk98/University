using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UserTier.Models
{
    public class ProductModel
    {

        [Display(Name = "Product ID")]
        [Required(ErrorMessage = "Enter product ID.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Enter product name.")]
        [Display(Name = "Product Name")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter product price.")]
        [Display(Name = "Product Price")]
        [DataType(DataType.Currency)]
        public int Price { get; set; }

        [Required(ErrorMessage = "Enter stock number.")]
        [Display(Name = "Product Stock")]
        [Range(1, 9999999999, ErrorMessage = "Stock needs to be between 1 and 9999999999")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Select product category.")]
        [Display(Name = "Product Category")]
        public int Category { get; set; }

        [Required(ErrorMessage = "Select product offer.")]
        [Display(Name = "Product Offer")]
        public int Offer { get; set; }

        [Display(Name = "Free Delivery")]
        public bool Delivery { get; set; }
    }
}