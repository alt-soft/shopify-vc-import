using System;
using System.Runtime.Serialization;
using Altsoft.ShopifyImportModule.Data.Models.Shopify.Base;

namespace Altsoft.ShopifyImportModule.Data.Models.Shopify
{
    [DataContract]
    public class ShopifyImage : ShopifyCreatableEntity
    {
        [DataMember(Name = "position")]
        public int Position { get; set; }

        [DataMember(Name = "product_id")]
        public long ProductId { get; set; }

        [DataMember(Name = "variant_ids")]
        public long[] VariantIds { get; set; }

        [DataMember(Name = "src")]
        public string Src { get; set; }
    }
}