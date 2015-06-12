using System;
using System.Collections.Generic;
using System.Linq;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;

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

            if (importParams.IsRetainCategoryHierarchy)
            {
                foreach (var shopifyCategory in _shopifyRepository.GetShopifyCollections())
                {
                    AddToVirtoCategoriesList(shopifyCategory, importParams.VirtoCategoryId,
                        virtoCategories);
                }
                foreach (var virtoCategory in virtoCategories)
                {
                    virtoCategory.CatalogId = importParams.VirtoCatalogId;
                }

                foreach (var virtoCategory in virtoCategories)
                {
                    _virtoRepository.AddCategory(virtoCategory);
                }
            }

            shopifyImportProgress.CurrentOperationDescription = "Adding items…";
            _shopifyImportProgressService.StoreCurrentProgress(shopifyImportProgress);

            var shopifyCollects = _shopifyRepository.GetShopifyCollects().ToList();

            try
            {
                foreach (var product in productsToImport)
                {
                    var virtoProduct = _shopifyConverter.Convert(product);
                    
                    IEnumerable<string> virtoCategoryIds;
                    if (importParams.IsRetainCategoryHierarchy && shopifyCollects.Any(collect => collect.ProductId == product.Id))
                    {
                        virtoCategoryIds =
                            shopifyCollects.Where(collect => collect.ProductId == product.Id)
                                .Select(
                                    collect =>
                                        virtoCategories.First(
                                            virtoCategory => virtoCategory.ShopifyId == collect.CollectionId).VirtoId);
                    }
                    else
                    {
                        virtoCategoryIds = !string.IsNullOrEmpty(importParams.VirtoCategoryId)
                            ? new List<string> {importParams.VirtoCategoryId}
                            : null;
                    }
                    
                    _virtoRepository.AddProduct(virtoProduct, importParams.VirtoCatalogId, virtoCategoryIds);

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