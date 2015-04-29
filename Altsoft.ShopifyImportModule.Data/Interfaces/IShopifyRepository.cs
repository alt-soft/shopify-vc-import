using System.Collections.Generic;
using Altsoft.ShopifyImportModule.Data.Models;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IShopifyRepository
    {
        #region Public Methods

        PaginationResult<ShopifyProduct> GetShopifyProductsFromSource(ShopifyProductSearchCriteria searchCriteria);

        IEnumerable<ShopifyCategory> GetShopifyCategoriesTree();

        #endregion 
    }
}