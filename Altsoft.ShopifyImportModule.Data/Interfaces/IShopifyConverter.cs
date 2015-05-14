using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IShopifyConverter
    {
        //ShopifyProduct Convert(catalogProductEntity product);
        Product Convert(ShopifyProduct product);

        //ShopifyCategory Convert(catalogCategoryEntity category);
        Category Convert(ShopifyCustomCollection category);
    }
}