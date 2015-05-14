using System.Runtime.Serialization;

namespace Altsoft.ShopifyImportModule.Data.Models.Shopify
{
    [DataContract]
    public class ShopifyCustomCollectionList
    {
        [DataMember(Name = "custom_collections")]
        public ShopifyCustomCollection[] CustomCollections { get; set; }
    }
}