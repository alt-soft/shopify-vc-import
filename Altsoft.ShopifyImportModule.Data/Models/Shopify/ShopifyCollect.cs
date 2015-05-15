﻿using System.Runtime.Serialization;
using Altsoft.ShopifyImportModule.Data.Models.Shopify.Base;

namespace Altsoft.ShopifyImportModule.Data.Models.Shopify
{
    [DataContract]
    public class ShopifyCollect:ShopifyCreatableEntity
    {
        [DataMember(Name = "collection_id")]
        public long CollectionId { get; set; }

        [DataMember(Name = "featured")]
        public bool Featured { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }

        [DataMember(Name = "product_id")]
        public long ProductId { get; set; }

        [DataMember(Name = "sort_value")]
        public string SortValue { get; set; }
    }
}