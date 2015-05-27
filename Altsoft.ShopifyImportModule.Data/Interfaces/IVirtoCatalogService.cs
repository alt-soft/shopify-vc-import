using Altsoft.ShopifyImportModule.Data.Models;
using VirtoCommerce.Domain.Catalog.Model;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IVirtoCatalogService
    {
        PaginationResult<Catalog> GetCatalogs();
        PaginationResult<VirtoCommerce.Domain.Catalog.Model.Category> GetCategories(VirtoCategorySearchCriteria searchCriteria); 
    }
}