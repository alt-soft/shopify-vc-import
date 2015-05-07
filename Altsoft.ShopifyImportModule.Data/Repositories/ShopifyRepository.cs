using System;
using System.Collections.Generic;
using System.Diagnostics;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using VirtoCommerce.Platform.Core.Settings;

namespace Altsoft.ShopifyImportModule.Data.Repositories
{
    public class ShopifyRepository:IShopifyRepository
    {
        public PaginationResult<ShopifyProduct> GetShopifyProductsFromSource(ShopifyProductSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ShopifyCategory> GetShopifyCategoriesTree()
        {
          
            throw new NotImplementedException();
        }
    }
}