using System.Collections.Generic;
using Altsoft.ShopifyImportModule.Data.Models;
using VirtoCommerce.Domain.Catalog.Model;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IVirtoRepository
    {
        IEnumerable<CatalogProduct> GetCatalogs(VirtoCatalogSearchCriteria searchCriteria);
        IEnumerable<Category> GetCategories(VirtoCategorySearchCriteria searchCriteria);

        void AddProduct(CatalogProduct virtoProduct, string catalogId, IEnumerable<string> categoryIds);
        void AddCatalog(string catalogId);
        void AddCategory(Category virtoCategory);

        void CommitChanges();
    }
}