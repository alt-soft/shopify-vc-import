using System.ComponentModel.DataAnnotations;

namespace Altsoft.ShopifyImportModule.Web.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Shop name", Description = "Just the shop name part of shopname.myshopify.com")]
        public string ShopName { get; set; }

        [Required]
        [Display(Name = "Consumer key")]
        public string ConsumerKey { get; set; }
        [Required]
        [Display(Name = "Consumer secret")]
        public string ConsumerSecret { get; set; }
    }
}