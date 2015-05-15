using System.Runtime.Serialization;

namespace Altsoft.ShopifyImportModule.Data.Models.Shopify.Base
{
    [DataContract]
    public class ShopifyEntity
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
    }
}