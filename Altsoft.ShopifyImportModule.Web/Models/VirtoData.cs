using System.Collections.Generic;
using System.IO;
using Altsoft.ShopifyImportModule.Web.Models.Shopify;
using VirtoCommerce.Domain.Catalog.Model;

namespace Altsoft.ShopifyImportModule.Web.Models
{
    public class VirtoData
    {
        public List<CatalogProduct> Products { get; set; }
        public List<Category> Categories { get; set; }
        public Dictionary<ShopifyTheme, Stream> Themes { get; set; }
    }
}