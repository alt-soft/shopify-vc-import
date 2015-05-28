using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IShopifyService
    {
        PaginationResult<ShopifyProductItem> GetShopifyCollections();
    }
}