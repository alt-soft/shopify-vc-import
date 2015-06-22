using System;
using System.Collections.Generic;
using System.Linq;
using Altsoft.ShopifyImportModule.Data.Converters;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;

namespace Altsoft.ShopifyImportModule.Data.Services
{
    public class ShopifyService : IShopifyService
    {
        private readonly IShopifyRepository _shopifyRepository;
        public ShopifyService(IShopifyRepository shopifyRepository)
        {
            _shopifyRepository = shopifyRepository;
        }

        public PaginationResult<ShopifyProductItem> GetShopifyCollections()
        {
          throw new NotImplementedException();
        }
    }
}