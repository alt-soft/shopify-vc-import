using System.Runtime.Serialization;

namespace Altsoft.ShopifyImportModule.Data.Models
{
    [DataContract]
    public class ShopifyProductList
    {
        [DataMember(Name = "products")]
        public ShopifyProduct[] Products { get; set; }
    }
}