using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class AppUser:IdentityUser
    {
        [StringLength(100)]
        [MaxLength(100)]
        [Required]
        public String? Name { get; set; }

        public String? Adress { get; set; }
    }
}
