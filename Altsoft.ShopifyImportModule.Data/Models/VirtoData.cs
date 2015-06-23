using System.Collections.Generic;
using VirtoCommerce.Domain.Catalog.Model;

namespace Altsoft.ShopifyImportModule.Data.Models
{
    public class VirtoData
    {
        public List<CatalogProduct> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}