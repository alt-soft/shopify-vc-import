using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.Notification;
using coreModel = VirtoCommerce.Domain.Catalog.Model;

namespace Altsoft.ShopifyImportModule.Data.Services
{
    public class ShopifyImportService : IShopifyImportService
    {
        const int NotifyProductSizeLimit = 10;

        #region Private Fields

        private readonly IShopifyRepository _shopifyRepository;
        private readonly IShopifyConverter _shopifyConverter;
        private readonly INotifier _notifier;
        private readonly IItemService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogSearchService _searchService;
        private readonly IThemeService _themeService;
        private readonly IPricingService _pricingService;

        #endregion

        #region Constructors

        public ShopifyImportService(
            IShopifyRepository shopifyRepository,
            IShopifyConverter shopifyConverter,
            INotifier notifier,
            IItemService productService,
            ICategoryService categoryService,
            ICatalogSearchService searchService,
            IThemeService themeService, IPricingService pricingService)
        {

            _shopifyRepository = shopifyRepository;
            _shopifyConverter = shopifyConverter;
            _notifier = notifier;
            _productService = productService;
            _categoryService = categoryService;
            _searchService = searchService;
            _themeService = themeService;
            _pricingService = pricingService;
        }

        #endregion

        public ShopifyImportNotification Import(ShopifyImportParams importParams, ShopifyImportNotification notification)
        {
            try
            {
                var shopifyData = ReadData(importParams, notification);
                var virtoData = ConvertData(shopifyData, importParams, notification);
                SaveData(virtoData, importParams, notification);
            }
            catch (Exception ex)
            {
                notification.Description = "Import error";
                notification.ErrorCount++;
                notification.Errors.Add(ex.ToString());
            }
            finally
            {
                notification.Finished = DateTime.UtcNow;
                notification.Description = "Import finished" + (notification.Errors.Any() ? " with errors" : " successfully");
                _notifier.Upsert(notification);
            }

            return notification;
        }

        private void SaveData(VirtoData virtoData, ShopifyImportParams importParams,
            ShopifyImportNotification notification)
        {
            if (importParams.ImportCollections)
            {
                SaveCategories(virtoData, importParams, notification);
            }

            if (importParams.ImportProducts)
            {
                SaveProducts(virtoData, importParams, notification);
            }

            if (importParams.ImportThemes)
            {
                SaveThemes(virtoData, importParams, notification);
            }
        }

        private void SaveThemes(VirtoData virtoData, ShopifyImportParams importParams, ShopifyImportNotification notification)
        {
            notification.TotalCount = virtoData.Themes.Count;
            notification.ProcessedCount = 0;
            notification.Description = "Saving themes";
            _notifier.Upsert(notification);

            foreach (var theme in virtoData.Themes)
            {
                _themeService.UploadTheme(importParams.StoreId, theme.Key.Name, theme.Value);
                notification.ProcessedCount++;
                _notifier.Upsert(notification);
            }
        }

        private void SaveProducts(VirtoData virtoData, ShopifyImportParams importParams, ShopifyImportNotification notification)
        {
            notification.ProcessedCount = 0;
            notification.TotalCount = virtoData.Products.Count;

            if (importParams.ImportCollections)
            {
                //Update categories references
                foreach (var product in virtoData.Products)
                {
                    if (product.Category != null)
                    {
                        product.Category = virtoData.Categories.First(category => category.Code == product.Category.Code);
                        product.CategoryId = product.Category.Id;
                    }
                }
            }

            notification.Description = "Checking existing products";
            _notifier.Upsert(notification);

            var existingProducts = _searchService.Search(new coreModel.SearchCriteria()
            {
                CatalogId = importParams.VirtoCatalogId,
                GetAllCategories = true,
                ResponseGroup = coreModel.ResponseGroup.WithProducts
            }).Products;

            notification.Description = "Saving products";
            _notifier.Upsert(notification);

            var productsToCreate = new List<coreModel.CatalogProduct>();
            var productsToUpdate = new List<coreModel.CatalogProduct>();

            foreach (var product in virtoData.Products)
            {
                var existingProduct = existingProducts.FirstOrDefault(p => p.Code == product.Code);

                if (existingProduct != null)
                {
                    product.Id = existingProduct.Id;
                    productsToUpdate.Add(product);
                }
                else
                {
                    productsToCreate.Add(product);
                }
            }

            foreach (var product in productsToCreate)
            {
                try
                {
                    _productService.Create(product);

                    //Create price in default price list
                    if (product.Prices != null && product.Prices.Any())
                    {
                        var price = product.Prices.First();
                        price.ProductId = product.Id;
                        _pricingService.CreatePrice(price);
                    }
                }
                catch (Exception ex)
                {
                    notification.ErrorCount++;
                    notification.Errors.Add(ex.ToString());
                    _notifier.Upsert(notification);
                }
                finally
                {
                    //Raise notification each notifyProductSizeLimit category
                    notification.ProcessedCount++;
                    notification.Description = string.Format("Creating products: {0} of {1} created",
                        notification.ProcessedCount, notification.TotalCount);
                    if (notification.ProcessedCount % NotifyProductSizeLimit == 0 || notification.ProcessedCount + notification.ErrorCount == notification.TotalCount)
                    {
                        _notifier.Upsert(notification);
                    }
                }
            }


            if (productsToUpdate.Count > 0)
            {
                try
                {
                    _productService.Update(productsToUpdate.ToArray());
                }
                catch (Exception ex)
                {
                    notification.ErrorCount++;
                    notification.Errors.Add(ex.ToString());
                    _notifier.Upsert(notification);
                }
                finally
                {
                    notification.Description = string.Format("Updating products: {0} updated",
                     notification.ProcessedCount = notification.ProcessedCount + productsToUpdate.Count);
                    _notifier.Upsert(notification);
                }
            }

            virtoData.Products.Clear();
            virtoData.Products.AddRange(productsToCreate);
            virtoData.Products.AddRange(productsToUpdate);

        }

        private void SaveCategories(VirtoData virtoData, ShopifyImportParams importParams,
            ShopifyImportNotification notification)
        {


            notification.ProcessedCount = 0;
            notification.TotalCount = virtoData.Categories.Count;

            notification.Description = "Saving categories";
            _notifier.Upsert(notification);

            var categoriesToCreate = new List<coreModel.Category>();
            var categoriesToUpdate = new List<coreModel.Category>();

            var existingCategories = _searchService.Search(new coreModel.SearchCriteria()
            {
                CatalogId = importParams.VirtoCatalogId,
                GetAllCategories = true,
                ResponseGroup = coreModel.ResponseGroup.WithCategories
            }).Categories;

            foreach (var category in virtoData.Categories)
            {
                var existingCategory = existingCategories.FirstOrDefault(c => c.Code == category.Code);
                if (existingCategory != null)
                {
                    category.Id = existingCategory.Id;
                    categoriesToUpdate.Add(category);
                }
                else
                {
                    categoriesToCreate.Add(category);
                }
            }

            foreach (var category in categoriesToCreate)
            {
                try
                {
                    _categoryService.Create(category);
                }
                catch (Exception ex)
                {
                    notification.ErrorCount++;
                    notification.Errors.Add(ex.ToString());
                    _notifier.Upsert(notification);
                }
                finally
                {
                    //Raise notification each notifyProductSizeLimit category
                    notification.ProcessedCount++;
                    notification.Description = string.Format("Creating categories: {0} of {1} created",
                        notification.ProcessedCount, notification.TotalCount);
                    if (notification.ProcessedCount % NotifyProductSizeLimit == 0 || notification.ProcessedCount + notification.ErrorCount == notification.TotalCount)
                    {
                        _notifier.Upsert(notification);
                    }
                }
            }

            if (categoriesToUpdate.Count > 0)
            {
                _categoryService.Update(categoriesToUpdate.ToArray());
                virtoData.Categories.AddRange(categoriesToUpdate);

                notification.Description = string.Format("Updating categories: {0} updated",
                    notification.ProcessedCount = notification.ProcessedCount + categoriesToUpdate.Count);
                _notifier.Upsert(notification);
            }

            virtoData.Categories.Clear();
            virtoData.Categories.AddRange(categoriesToCreate);
            virtoData.Categories.AddRange(categoriesToUpdate);
        }

        private VirtoData ConvertData(ShopifyData shopifyData, ShopifyImportParams importParams, ShopifyImportNotification notification)
        {
            var virtoData = new VirtoData();

            if (importParams.ImportCollections)
            {
                notification.Description = "Converting categories";
                _notifier.Upsert(notification);

                virtoData.Categories =
                    shopifyData.Collections
                    .Select(collection => _shopifyConverter.Convert(collection, importParams)).ToList();
            }

            if (importParams.ImportProducts)
            {
                notification.Description = "Converting products";
                _notifier.Upsert(notification);

                virtoData.Products =
                    shopifyData.Products.Select(product => _shopifyConverter.Convert(product, importParams, shopifyData)).ToList();
            }

            if (importParams.ImportThemes)
            {
                virtoData.Themes = shopifyData.Themes;
            }

            return virtoData;
        }

        private ShopifyData ReadData(ShopifyImportParams importParams, ShopifyImportNotification notification)
        {
            var result = new ShopifyData();

            if (importParams.ImportProducts)
            {
                notification.Description = "Reading products from shopify...";
                _notifier.Upsert(notification);
                result.Products = _shopifyRepository.GetShopifyProducts();
            }

            if (importParams.ImportCollections)
            {
                notification.Description = "Reading collects from shopify...";
                _notifier.Upsert(notification);
                result.Collects = _shopifyRepository.GetShopifyCollects();

                notification.Description = "Reading collections from shopify...";
                _notifier.Upsert(notification);
                result.Collections = _shopifyRepository.GetShopifyCollections();
            }

            if (importParams.ImportThemes)
            {
                result.Themes = new Dictionary<ShopifyTheme, ZipArchive>();
                notification.Description = "Reading themes from shopify...";
                _notifier.Upsert(notification);
                var themes = _shopifyRepository.GetShopifyThemes().ToList();

                notification.ProcessedCount = 0;
                notification.TotalCount = themes.Count();
                foreach (var theme in themes)
                {
                    notification.Description = string.Format("Reading theme '{0}' assets from shopify...", theme.Name);
                    _notifier.Upsert(notification);
                    var zip = _shopifyRepository.GetShopifyThemeZip(theme.Id);
                    notification.ProcessedCount++;
                    result.Themes.Add(theme, zip);
                }
            }

            //TODO read another data
            return result;
        }


    }
}