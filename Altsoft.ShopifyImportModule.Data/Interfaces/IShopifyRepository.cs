using System.Collections.Generic;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IShopifyRepository
    {
        #region Public Methods

        PaginationResult<ShopifyProduct> GetShopifyProductsFromSource(ShopifyProductSearchCriteria searchCriteria);

        PaginationResult<ShopifyCustomCollection> GetShopifyCollections();

        #endregion 
    }
}