using System;
using System.Collections.Generic;
using System.Linq;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Common;

namespace Altsoft.ShopifyImportModule.Data.Converters
{
    public class ShopifyConverter:IShopifyConverter
    {
        private ItemAsset Convert(ShopifyImage image, CatalogProduct item, bool isMain)
        {
            var retVal = new ItemAsset
            {
                CreatedDate = image.CreatedAt,
                ItemId = item.Id,
                ModifiedDate = image.UpdatedAt,
                Group = isMain ? "primaryimage" : "images",
                Url = image.Src,
                Type = ItemAssetType.Image,
            };

            return retVal;
        }

        public Category Convert(ShopifyCustomCollection category, ShopifyImportParams importParams)
        {
            var result = new Category
            {
                Code = category.Handle,
                Name = category.Title,
                Id = Guid.NewGuid().ToString(),
                CatalogId = importParams.VirtoCatalogId
            };

            return result;
        }

        public CatalogProduct Convert(ShopifyProduct product, ShopifyImportParams importParams, ShopifyData shopifyData)
        {
            var result = new CatalogProduct
            {
                Name = product.Title,
                Code = product.Handle,
                CreatedDate = DateTime.Now,
                StartDate = product.PublishedAt,
                IsActive = true,
                CatalogId = importParams.VirtoCatalogId
            };

            if (importParams.ImportImages && product.Images != null)
            {
                result.Assets = new List<ItemAsset>();
                result.Assets.AddRange(product.Images.Select(image => Convert(image, result, false)));
            }

            if (importParams.ImportCollections)
            {
                var firstCollect = shopifyData.Collects.FirstOrDefault(collect => collect.ProductId == product.Id);
                if (firstCollect != null)
                {
                    result.Category = new Category()
                    {
                        Code =
                            shopifyData.Collections.First(collection => collection.Id == firstCollect.CollectionId)
                                .Handle
                    };
                }
            }

            return result;
        }
    }
}