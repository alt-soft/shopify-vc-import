using System;
using System.Runtime.Serialization;

namespace Altsoft.ShopifyImportModule.Data.Models
{
    public class ShopifyImage
    {
        [DataMember(Name = "createdat")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }

        [DataMember(Name = "productid")]
        public long ProductId { get; set; }

        [DataMember(Name = "variantids")]
        public long[] VariantIds { get; set; }

        [DataMember(Name = "src")]
        public string Src { get; set; }

        [DataMember(Name = "updatedat")]
        public DateTime UpdatedAt { get; set; }

    }
}