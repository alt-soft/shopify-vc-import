using System;
using System.Runtime.Serialization;

namespace Altsoft.ShopifyImportModule.Data.Models.Shopify.Base
{
    [DataContract]
    public class ShopifyCreatableEntity:ShopifyUpdatableEntity
    {
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }    
    }
}