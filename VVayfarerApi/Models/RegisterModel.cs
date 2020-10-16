using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Your name must contain less than 50 characters.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(50, ErrorMessage = "Your email must contain less than 50 characters.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Your password must contain less than 50 characters.")]
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "Passwords are not the same.")]
        public string ConfirmPassword { get; set; }
    }
}
