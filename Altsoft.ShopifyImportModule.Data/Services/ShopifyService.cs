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
        private readonly IVirtoCatalogService _virtoCatalogService;
        public ShopifyService(IShopifyRepository shopifyRepository, IVirtoCatalogService virtoCatalogService)
        {
            _shopifyRepository = shopifyRepository;
            _virtoCatalogService = virtoCatalogService;
        }

        public PaginationResult<ShopifyProductItem> GetShopifyCollections()
        {
            PaginationResult<ShopifyProductItem> result;
            var products = _shopifyRepository.GetShopifyProductsFromSource(new ShopifyProductSearchCriteria());
            if (products == null)
            {
                result = new PaginationResult<ShopifyProductItem>()
                {
                    IsSuccess = false,
                    ErrorMessage = "Error getting shopify products. See log for details."
                };
            }
            else
            {
                var collections = _shopifyRepository.GetShopifyCollections();

                if (collections == null)
                {
                    result = new PaginationResult<ShopifyProductItem>()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Error getting shopify collections. See log for details."
                    };
                }
                else
                {
                    var collects = _shopifyRepository.GetShopifyCollects();
                    if (collects == null)
                    {
                        result = new PaginationResult<ShopifyProductItem>()
                        {
                            IsSuccess = false,
                            ErrorMessage = "Error getting shopify collects. See log for details."
                        };
                    }
                    else
                    {
                        var productList = products as IList<ShopifyProduct> ?? products.ToList();

                        var productHandles = productList.Select(product => product.Handle).ToList();
                        var existingProductsHandles = _virtoCatalogService.CheckItemsCodesExistance(productHandles);

                        foreach (var product in productList)
                        {
                            product.IsImported = existingProductsHandles.Contains(product.Handle);
                        }

                        var productItems =
                            collections.Select(
                                collection => ShopifyProductItemConverter.Convert(collection, collects, productList))
                                .ToList();

                        

                        result = new PaginationResult<ShopifyProductItem>()
                        {
                            IsSuccess = true,
                            Items = productItems,
                            TotalCount = productItems.Count()
                        };
                    }
                }
            }


            return result;
        }
    }
}