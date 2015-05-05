
using System.ComponentModel.DataAnnotations;

namespace Altsoft.ShopifyImportModule.Data.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Shop name", Description = "Just the shop name part of shopname.myshopify.com")]
        public string ShopName { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}