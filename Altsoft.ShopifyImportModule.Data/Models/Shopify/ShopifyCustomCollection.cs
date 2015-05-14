using System;
using System.Runtime.Serialization;

namespace Altsoft.ShopifyImportModule.Data.Models.Shopify
{
    [DataContract]
    public class ShopifyCustomCollection
    {
        [DataMember(Name = "body_html")]
        public string BodyHtml { get; set; }

        [DataMember(Name = "handle")]
        public string Handle { get; set; }

        [DataMember(Name = "image")]
        public string Image { get; set; }

        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "metafield")]
        public ShopifyMetafield Metafield { get; set; }

        [DataMember(Name = "published")]
        public bool Published { get; set; }

        [DataMember(Name = "published_at")]
        public DateTime PublishedAt { get; set; }

        [DataMember(Name = "published_scope")]
        public string PublishedScope { get; set; }

        [DataMember(Name = "sort_order")]
        public string SortOrder { get; set; }

        [DataMember(Name = "template_suffix")]
        public string TemplateSuffix { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }


    }
}