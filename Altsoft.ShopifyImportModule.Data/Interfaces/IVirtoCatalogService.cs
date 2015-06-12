using System.Collections.Generic;
using Altsoft.ShopifyImportModule.Data.Models;
using VirtoCommerce.CatalogModule.Data.Model;
using Catalog = VirtoCommerce.Domain.Catalog.Model.Catalog;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IVirtoCatalogService
    {
        PaginationResult<Catalog> GetCatalogs();
        PaginationResult<VirtoCommerce.Domain.Catalog.Model.Category> GetCategories(VirtoCategorySearchCriteria searchCriteria);
        void AddCategory(VirtoCategory virtoCategory);
        void AddProduct(Product virtoProduct, string virtoCatalogId, IEnumerable<string> virtoCategoryIds);
        void CommitChanges();
    }
}