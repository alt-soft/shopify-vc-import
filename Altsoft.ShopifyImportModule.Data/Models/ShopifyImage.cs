using System;
using System.Runtime.Serialization;

namespace Altsoft.ShopifyImportModule.Data.Models
{
    [DataContract]
    public class ShopifyImage
    {
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }

        [DataMember(Name = "product_id")]
        public long ProductId { get; set; }

        [DataMember(Name = "variant_ids")]
        public long[] VariantIds { get; set; }

        [DataMember(Name = "src")]
        public string Src { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }

    }
}