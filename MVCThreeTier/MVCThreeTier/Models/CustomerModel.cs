using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace MVCThreeTier.Models
{
    public class CustomerModel
    {
        [Display(Name = "Customer ID")]
        [Required(ErrorMessage = "Enter customer ID.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Enter first name.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter last name.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Enter home address.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Store Loyalty Card")]
        public int LoyaltyCard { get; set; }
    }
}