using System.Runtime.Serialization;

namespace Altsoft.ShopifyImportModule.Data.Models.Shopify
{
    [DataContract]
    public class ShopifyCollectList
    {
        [DataMember(Name = "collects")]
        public ShopifyCollect[] Collects { get; set; }
    }
}