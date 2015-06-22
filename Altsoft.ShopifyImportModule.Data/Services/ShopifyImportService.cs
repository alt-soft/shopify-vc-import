using System;
using System.Collections.Generic;
using System.Linq;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using VirtoCommerce.CatalogModule.Data.Model;

namespace Altsoft.ShopifyImportModule.Data.Services
{
    public class ShopifyImportService:IShopifyImportService
    {
      #region Private Fields

        private readonly IShopifyRepository _shopifyRepository;
        private readonly IShopifyConverter _shopifyConverter;
        private readonly IShopifyImportProgressService _shopifyImportProgressService;
        private readonly ILoggerFacade _loggerFacade;

        #endregion

        #region Constructors

        public ShopifyImportService(
            IShopifyRepository shopifyRepository,
            IShopifyConverter shopifyConverter,
            IShopifyImportProgressService shopifyImportProgressService, ILoggerFacade loggerFacade)
        {

            _shopifyRepository = shopifyRepository;
            _shopifyConverter = shopifyConverter;
            _shopifyImportProgressService = shopifyImportProgressService;
            _loggerFacade = loggerFacade;
        }

        #endregion

        #region IShopifyImportService Implementation

        public ServiceResponseBase Import(ShopifyImportParams importParams)
        {
         throw new NotImplementedException();
        }

        #endregion

        #region Auxiliary Methods

        private void AddToVirtoCategoriesList(ShopifyCustomCollection shopifyCategory, string virtoParentCategoryId,
             ICollection<VirtoCategory> virtoCategories)
        {
            var virtoCategory = _shopifyConverter.Convert(shopifyCategory);
            virtoCategory.ParentCategoryId = virtoParentCategoryId;

            virtoCategories.Add(virtoCategory);
        }

        #endregion
    }
}