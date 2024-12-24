using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Compare("Password",ErrorMessage ="Password don't match")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]

        public string? ConfirmPassword { get; set; }
        [DataType(DataType.MultilineText)]
        public string? Adresse { get; set; }

        [Display(Name = "Role")]
        [NotMapped]
        public string? Role { get; set; }


    }
}
