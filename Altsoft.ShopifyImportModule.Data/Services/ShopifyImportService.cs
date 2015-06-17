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

        private readonly IVirtoCatalogService _virtoRepository;
        private readonly IShopifyRepository _shopifyRepository;
        private readonly IShopifyConverter _shopifyConverter;
        private readonly IShopifyImportProgressService _shopifyImportProgressService;
        private readonly ILoggerFacade _loggerFacade;

        #endregion

        #region Constructors

        public ShopifyImportService(
            IVirtoCatalogService virtoRepository,
            IShopifyRepository shopifyRepository,
            IShopifyConverter shopifyConverter,
            IShopifyImportProgressService shopifyImportProgressService, ILoggerFacade loggerFacade)
        {

            _virtoRepository = virtoRepository;
            _shopifyRepository = shopifyRepository;
            _shopifyConverter = shopifyConverter;
            _shopifyImportProgressService = shopifyImportProgressService;
            _loggerFacade = loggerFacade;
        }

        #endregion

        #region IShopifyImportService Implementation

        public ServiceResponseBase Import(ShopifyImportParams importParams)
        {
            IEnumerable<ShopifyProduct> productsToImport;

            List<ShopifyCollect> shopifyCollects;
            
            try
            {
                shopifyCollects = _shopifyRepository.GetShopifyCollects().ToList();
            }
            catch (Exception e)
            {
                _loggerFacade.Log(e.Message + e.StackTrace, LogCategory.Exception, LogPriority.High);
                return new ServiceResponseBase
                {
                    IsSuccess = false,
                    ErrorMessage = "Can't retrieve Shopify collects. See log files for more information."
                };
            }

            try
            {
                productsToImport =
                    _shopifyRepository.GetShopifyProductsFromSource(new ShopifyProductSearchCriteria()).Where(
                        product => importParams.ShopifyProductIds.Contains(product.Id)).ToList();
            }
            catch (Exception e)
            {
                _loggerFacade.Log(e.Message + e.StackTrace, LogCategory.Exception, LogPriority.High);
                return new ServiceResponseBase
                {
                    IsSuccess = false,
                    ErrorMessage = "Can't retrieve Shopify products. See log files for more information."
                };
            }

            var shopifyImportProgress = new ShopifyImportProgress
            {
                AddedItemsCount = 0,
                TotalItemsCount = productsToImport.Count(),
                CurrentOperationDescription = "Adding categories…"
            };
            _shopifyImportProgressService.StoreCurrentProgress(shopifyImportProgress);

            var virtoCategories = new List<VirtoCategory>();
            var newCategories = new List<CategoryBase>();
            if (importParams.IsRetainCategoryHierarchy)
            {
                var collectionsToImport =
                    productsToImport.Select(
                        product => shopifyCollects.First(collect => collect.ProductId == product.Id).CollectionId)
                        .ToList();

                foreach (var shopifyCategory in _shopifyRepository.GetShopifyCollections())
                {
                    if (collectionsToImport.Contains(shopifyCategory.Id))
                        AddToVirtoCategoriesList(shopifyCategory, importParams.VirtoCategoryId,
                            virtoCategories);
                }

                foreach (var virtoCategory in virtoCategories)
                {
                    virtoCategory.CatalogId = importParams.VirtoCatalogId;
                }

                var shopifyCollectionCodes = virtoCategories.Select(category => category.Code).ToList();

                var existingCategoriesCodes =
                    _virtoRepository.CheckCategoriesCodesExistance(shopifyCollectionCodes);

                newCategories.AddRange(virtoCategories.Where(category=>!existingCategoriesCodes.Contains(category.Code)).Select(virtoCategory => _virtoRepository.AddCategory(virtoCategory)));
            }

            shopifyImportProgress.CurrentOperationDescription = "Adding items…";
            _shopifyImportProgressService.StoreCurrentProgress(shopifyImportProgress);

            

            try
            {
                foreach (var product in productsToImport)
                {
                    var virtoProduct = _shopifyConverter.Convert(product);
                    
                    IEnumerable<string> virtoCategoryCodes;
                    if (importParams.IsRetainCategoryHierarchy && shopifyCollects.Any(collect => collect.ProductId == product.Id))
                    {
                        virtoCategoryCodes =
                            shopifyCollects.Where(collect => collect.ProductId == product.Id)
                                .Select(
                                    collect =>
                                        virtoCategories.First(
                                            virtoCategory => virtoCategory.ShopifyId == collect.CollectionId).Code);
                    }
                    else
                    {
                        virtoCategoryCodes = !string.IsNullOrEmpty(importParams.VirtoCategoryId)
                            ? new List<string> {importParams.VirtoCategoryId}
                            : null;
                    }

                    virtoProduct.CatalogId = importParams.VirtoCatalogId;

                    _virtoRepository.AddProduct(virtoProduct, newCategories, virtoCategoryCodes);

                    shopifyImportProgress.AddedItemsCount++;
                    _shopifyImportProgressService.StoreCurrentProgress(shopifyImportProgress);
                }

                shopifyImportProgress.CurrentOperationDescription = "Saving changes…";
                _shopifyImportProgressService.StoreCurrentProgress(shopifyImportProgress);
                _virtoRepository.CommitChanges();
                shopifyImportProgress.CurrentOperationDescription = "Completed";
                _shopifyImportProgressService.StoreCurrentProgress(shopifyImportProgress);
            }
            catch (Exception e)
            {
                shopifyImportProgress.CurrentOperationDescription = "Failed";
                _shopifyImportProgressService.StoreCurrentProgress(shopifyImportProgress);

                _loggerFacade.Log(e.Message + e.StackTrace, LogCategory.Exception, LogPriority.High);

                return new ServiceResponseBase()
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to import products. See log files for more information."
                };
            }

            return new ServiceResponseBase
            {
                IsSuccess = true
            };
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