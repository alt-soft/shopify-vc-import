using System.Collections.Generic;
using System.IO.Compression;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using VirtoCommerce.Domain.Catalog.Model;

namespace Altsoft.ShopifyImportModule.Data.Models
{
    public class VirtoData
    {
        public List<CatalogProduct> Products { get; set; }
        public List<Category> Categories { get; set; }
        public Dictionary<ShopifyTheme, ZipArchive> Themes { get; set; }
    }
}