using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Database.Entities
{
    public class User
    {

        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name cant be more than 20 chararcters")]
        [StringLength(20, MinimumLength =3)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Surname cant be more than 30 characters")]
        [StringLength(30, MinimumLength =3)]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "Address cant be more than 30 characters")]
        [StringLength(30, MinimumLength = 5)]
        public string Address { get; set; }
        [Required(ErrorMessage ="Phone number must be a minimum of 11 numbers please start with 07")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11,MinimumLength =11)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage ="Must enter an email address and must be a minimum of 10 characters")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Must enter a password of at least length 7")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 7)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Must choose a level of clearnce")]
        public string UserType { get; set; }
    }
}
