using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using VirtoCommerce.Domain.Catalog.Model;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IShopifyConverter
    {
        Category Convert(ShopifyCustomCollection category, ShopifyImportParams importParams);
        CatalogProduct Convert(ShopifyProduct category, ShopifyImportParams importParams, ShopifyData shopifyData);
    }
}