using System.Collections.Generic;
using System.IO.Compression;

namespace Altsoft.ShopifyImportModule.Data.Models.Shopify
{
    public class ShopifyData
    {
        public IEnumerable<ShopifyProduct> Products { get; set; }
        public IEnumerable<ShopifyCollect> Collects { get; set; }
        public IEnumerable<ShopifyCustomCollection> Collections { get; set; }
        public Dictionary<ShopifyTheme, ZipArchive> Themes { get; set; } 
    }
}