using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace UserTier.Models
{
    public class CustomerModel
    {
        [Display(Name = "Customer ID")]
        [Required(ErrorMessage = "Enter customer ID.")]
        [Range(0, 999999, ErrorMessage = "ID must be between 0 and 999999")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Enter first name.")]
        [Display(Name = "First name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter last name.")]
        [Display(Name = "Last name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter email address.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter phone number.")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Enter home address.")]
        [Display(Name = "Address")]
        [MaxLength(30)]
        public string Address { get; set; }

        [Display(Name = "Store Loyalty Card")]
        public int LoyaltyCard { get; set; }
    }
}