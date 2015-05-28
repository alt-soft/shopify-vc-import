using System.Collections.Generic;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IShopifyRepository
    {
        #region Public Methods

        IEnumerable<ShopifyProduct> GetShopifyProductsFromSource(ShopifyProductSearchCriteria searchCriteria);

        IEnumerable<ShopifyCustomCollection> GetShopifyCollections();

        IEnumerable<ShopifyCollect> GetShopifyCollects();

        #endregion
    }
}