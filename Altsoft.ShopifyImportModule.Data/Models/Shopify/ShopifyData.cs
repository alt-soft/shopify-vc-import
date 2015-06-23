using System.Collections.Generic;

namespace Altsoft.ShopifyImportModule.Data.Models.Shopify
{
    public class ShopifyData
    {
        public IEnumerable<ShopifyProduct> Products { get; set; }
        public IEnumerable<ShopifyCollect> Collects { get; set; }
        public IEnumerable<ShopifyCustomCollection> Collections { get; set; }
    }
}