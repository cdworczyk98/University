using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UserTier.Models
{
    public class LoginModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Enter use Username.")]
        public string Username { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter Passsword")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}