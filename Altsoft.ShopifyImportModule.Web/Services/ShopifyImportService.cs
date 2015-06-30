using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Altsoft.ShopifyImportModule.Web.Interfaces;
using Altsoft.ShopifyImportModule.Web.Models;
using Altsoft.ShopifyImportModule.Web.Models.Shopify;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.Notification;
using coreModel = VirtoCommerce.Domain.Catalog.Model;

namespace Altsoft.ShopifyImportModule.Web.Services
{
    public class ShopifyImportService : IShopifyImportService
    {
        const int NotifySizeLimit = 1;
        private const string ProductsKey = "Products import";
        private const string CollectionsKey = "Collections import";
        private const string PropertiesKey = "Properties import";
        private const string ThemesKey = "Themes import";

        #region Private Fields

        private readonly IShopifyRepository _shopifyRepository;
        private readonly IShopifyConverter _shopifyConverter;
        private readonly INotifier _notifier;
        private readonly IItemService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogSearchService _searchService;
        private readonly IThemeService _themeService;
        private readonly IPricingService _pricingService;
        private readonly IPropertyService _propertyService;

        #endregion

        #region Constructors

        public ShopifyImportService(
            IShopifyRepository shopifyRepository,
            IShopifyConverter shopifyConverter,
            INotifier notifier,
            IItemService productService,
            ICategoryService categoryService,
            ICatalogSearchService searchService,
            IThemeService themeService, 
            IPricingService pricingService, 
            IPropertyService propertyService)
        {

            _shopifyRepository = shopifyRepository;
            _shopifyConverter = shopifyConverter;
            _notifier = notifier;
            _productService = productService;
            _categoryService = categoryService;
            _searchService = searchService;
            _themeService = themeService;
            _pricingService = pricingService;
            _propertyService = propertyService;
        }

        #endregion

        public ShopifyImportNotification Import(ShopifyImportParams importParams, ShopifyImportNotification notification)
        {
            try
            {
                var shopifyData = ReadData(importParams, notification);
                UpdateProgresses(importParams, notification, shopifyData);
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

        private void UpdateProgresses(ShopifyImportParams importParams, ShopifyImportNotification notification, ShopifyData shopifyData)
        {
            notification.Progresses.Clear();

            if (importParams.ImportProducts)
            {
                notification.Progresses.Add(ProductsKey, new ShopifyImportProgress()
                {
                    TotalCount = shopifyData.Products.Count()
                });

                notification.Progresses.Add(PropertiesKey, new ShopifyImportProgress()
                {
                    TotalCount = shopifyData.Products.SelectMany(product=>product.Options).GroupBy(o=>o.Name).Count()
                });
            }
            if (importParams.ImportCollections)
            {
                notification.Progresses.Add(CollectionsKey, new ShopifyImportProgress()
                {
                    TotalCount = shopifyData.Collections.Count()
                });
            }

            if (importParams.ImportThemes)
            {
                notification.Progresses.Add(ThemesKey, new ShopifyImportProgress()
                {
                    TotalCount = shopifyData.Themes.Count()
                });
            }

            _notifier.Upsert(notification);
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
                SaveProperties(virtoData, importParams, notification);
                SaveProducts(virtoData, importParams, notification);
            }

            if (importParams.ImportThemes)
            {
                SaveThemes(virtoData, importParams, notification);
            }
        }

        private void SaveProperties(VirtoData virtoData, ShopifyImportParams importParams,
            ShopifyImportNotification notification)
        {
            var propertiesProgress = notification.Progresses[PropertiesKey];

            notification.Description = "Saving properties";
            _notifier.Upsert(notification);

            var propertiesToCreate = new List<coreModel.Property>();
            var propertiesToUpdate = new List<coreModel.Property>();

            var existingProperties = _propertyService.GetCatalogProperties(importParams.VirtoCatalogId);

            foreach (var property in virtoData.Properties)
            {
                if (propertiesToCreate.All(p => p.Name != property.Name) &&
                    propertiesToUpdate.All(p => p.Name != property.Name))
                {
                    var existitngProperty = existingProperties.FirstOrDefault(c => c.Name == property.Name);
                    if (existitngProperty != null)
                    {
                        property.Id = existitngProperty.Id;
                        propertiesToUpdate.Add(property);
                    }
                    else
                    {
                        propertiesToCreate.Add(property);
                    }    
                }
                
            }

            foreach (var property in propertiesToCreate)
            {
                try
                {
                    _propertyService.Create(property);
                }
                catch (Exception ex)
                {
                    propertiesProgress.ErrorCount++;
                    notification.ErrorCount++;
                    notification.Errors.Add(ex.ToString());
                    _notifier.Upsert(notification);
                }
                finally
                {
                    //Raise notification each notifyProductSizeLimit property
                    propertiesProgress.ProcessedCount++;
                    notification.Description = string.Format("Creating properties: {0} of {1} created",
                        propertiesProgress.ProcessedCount, propertiesProgress.TotalCount);
                    if (propertiesProgress.ProcessedCount%NotifySizeLimit == 0 ||
                        propertiesProgress.ProcessedCount + propertiesProgress.ErrorCount ==
                        propertiesProgress.TotalCount)
                    {
                        _notifier.Upsert(notification);
                    }
                }
            }

            if (propertiesToUpdate.Count > 0)
            {
                _propertyService.Update(propertiesToUpdate.ToArray());

                notification.Description = string.Format("Updating properties: {0} updated",
                    propertiesProgress.ProcessedCount = propertiesProgress.ProcessedCount + propertiesToUpdate.Count);
                _notifier.Upsert(notification);
            }
            virtoData.Properties.Clear();
            virtoData.Properties.AddRange(propertiesToCreate);
            virtoData.Properties.AddRange(propertiesToUpdate);
        }

        private void SaveThemes(VirtoData virtoData, ShopifyImportParams importParams, ShopifyImportNotification notification)
        {
            notification.Description = "Saving themes";
            _notifier.Upsert(notification);

            foreach (var theme in virtoData.Themes)
            {
                using (var zip = new ZipArchive(theme.Value,ZipArchiveMode.Read))
                {
                    _themeService.UploadTheme(importParams.StoreId, theme.Key.Name, zip);
                }
                
                
                notification.Progresses[ThemesKey].ProcessedCount++;
                _notifier.Upsert(notification);
            }
        }

        private void SaveProducts(VirtoData virtoData, ShopifyImportParams importParams, ShopifyImportNotification notification)
        {
            var productProgress = notification.Progresses[ProductsKey];
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


            notification.Description = "Saving products";
            _notifier.Upsert(notification);

            var productsToCreate = new List<coreModel.CatalogProduct>();
            var productsToUpdate = new List<coreModel.CatalogProduct>();

            foreach (var product in virtoData.Products)
            {
                var existingProduct = _searchService.Search(new coreModel.SearchCriteria()
                {
                    CatalogId = importParams.VirtoCatalogId,
                    GetAllCategories = true,
                    ResponseGroup = coreModel.ResponseGroup.WithProducts,
                    Count = int.MaxValue,
                    Code = product.Code
                }).Products.FirstOrDefault();

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
                    productProgress.ErrorCount++;
                    notification.ErrorCount++;
                    notification.Errors.Add(ex.ToString());
                    _notifier.Upsert(notification);
                }
                finally
                {
                    //Raise notification each notifyProductSizeLimit category
                    
                    productProgress.ProcessedCount++;
                    notification.Description = string.Format("Creating products: {0} of {1} created",
                        productProgress.ProcessedCount, productProgress.TotalCount);
                    if (productProgress.ProcessedCount % NotifySizeLimit == 0 || productProgress.ProcessedCount + productProgress.ErrorCount == productProgress.TotalCount)
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
                    productProgress.ErrorCount++;
                    notification.ErrorCount++;
                    notification.Errors.Add(ex.ToString());
                    _notifier.Upsert(notification);
                }
                finally
                {
                    notification.Description = string.Format("Updating products: {0} updated",
                     productProgress.ProcessedCount = productProgress.ProcessedCount + productsToUpdate.Count);
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
            var categoriesProgress = notification.Progresses[CollectionsKey];

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
                    categoriesProgress.ErrorCount++;
                    notification.ErrorCount++;
                    notification.Errors.Add(ex.ToString());
                    _notifier.Upsert(notification);
                }
                finally
                {
                    //Raise notification each notifyProductSizeLimit category
                    categoriesProgress.ProcessedCount++;
                    notification.Description = string.Format("Creating categories: {0} of {1} created",
                        categoriesProgress.ProcessedCount, categoriesProgress.TotalCount);
                    if (categoriesProgress.ProcessedCount % NotifySizeLimit == 0 || categoriesProgress.ProcessedCount + categoriesProgress.ErrorCount == categoriesProgress.TotalCount)
                    {
                        _notifier.Upsert(notification);
                    }
                }
            }

            if (categoriesToUpdate.Count > 0)
            {
                _categoryService.Update(categoriesToUpdate.ToArray());

                notification.Description = string.Format("Updating categories: {0} updated",
                    categoriesProgress.ProcessedCount = categoriesProgress.ProcessedCount + categoriesToUpdate.Count);
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
                notification.Description = "Converting product properties";
                _notifier.Upsert(notification);

                virtoData.Properties =
                    shopifyData.Products.SelectMany(product => product.Options)
                        .Select(option => _shopifyConverter.Convert(option, importParams))
                        .ToList();

                notification.Description = "Converting products";
                _notifier.Upsert(notification);

                virtoData.Products =
                    shopifyData.Products.Select(
                        product => _shopifyConverter.Convert(product, importParams, shopifyData, virtoData)).ToList();
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

                notification.Progresses.Add("Collections", new ShopifyImportProgress()
                {
                    TotalCount = result.Collections.Count()
                });
            }

            if (importParams.ImportThemes)
            {
                result.Themes = new Dictionary<ShopifyTheme, Stream>();
                notification.Description = "Reading themes from shopify...";
                var themes = _shopifyRepository.GetShopifyThemes().ToList();

                notification.Progresses.Clear();
                _notifier.Upsert(notification);
              
             
                foreach (var theme in themes)
                {
                    var assets = _shopifyRepository.GetShopifyAssets(theme.Id).ToList();

                    var themeProgress = new ShopifyImportProgress()
                    {
                        TotalCount = assets.Count
                    };
                    notification.Progresses.Add(string.Format("{0} theme assets downloading ", theme.Name),themeProgress);
                    _notifier.Upsert(notification);

                    var stream = new MemoryStream();
                    using (var zip = new ZipArchive(stream, ZipArchiveMode.Create,true))
                    {
                        foreach (var asset in assets)
                        {
                            try
                            {
                                var downloadedAsset = _shopifyRepository.DownloadShopifyAsset(theme.Id, asset);
                                var entry = zip.CreateEntry(string.Format("{0}/{1}", theme.Id, asset.Key));
                                using (var entryStream = entry.Open())
                                {
                                    if (downloadedAsset.Value != null)
                                    {
                                        using (var writer = new StreamWriter(entryStream))
                                        {
                                            writer.Write(downloadedAsset.Value);
                                        }
                                    }
                                    else
                                    {
                                        if (downloadedAsset.Attachment != null)
                                        {
                                            var data = Convert.FromBase64String(downloadedAsset.Attachment);
                                            entryStream.Write(data, 0, data.Length);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                themeProgress.ErrorCount++;
                                notification.ErrorCount++;
                                notification.Errors.Add(ex.ToString());
                                _notifier.Upsert(notification);
                            }
                            finally
                            {
                                //Raise notification each notifyProductSizeLimit category
                                themeProgress.ProcessedCount++;

                                if (themeProgress.ProcessedCount%NotifySizeLimit == 0 ||
                                    themeProgress.ProcessedCount + themeProgress.ErrorCount == themeProgress.TotalCount)
                                {
                                    _notifier.Upsert(notification);
                                }
                            }
                        }
                    }
                    stream.Seek(0, SeekOrigin.Begin);
                    result.Themes.Add(theme, stream);
                }
            }

            //TODO read another data
            return result;
        }
    }
}