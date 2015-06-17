using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Common;
using ItemAsset = VirtoCommerce.CatalogModule.Data.Model.ItemAsset;

namespace Altsoft.ShopifyImportModule.Data.Converters
{
    public class ShopifyConverter:IShopifyConverter
    {
        public Product Convert(ShopifyProduct product)
        {
            var result = new Product();
            result.Name = product.Title;
            result.Code = product.Handle;
            result.CreatedDate = DateTime.Now;
            result.StartDate = product.PublishedAt;
            result.IsActive = true;

            if (product.Images != null)
            {
                result.ItemAssets = new ObservableCollection<ItemAsset>();
                result.ItemAssets.AddRange(product.Images.Select(image => Convert(image, result, false)));
            }

            return result;
        }

        private ItemAsset Convert(ShopifyImage image, Item item, bool isMain)
        {
            var retVal = new ItemAsset
            {
                CreatedDate = image.CreatedAt,
                AssetType = ItemAssetType.Image.ToString().ToLower(),
                CatalogItem = item,
                ItemId = item.Id,
                ModifiedDate = image.UpdatedAt,
                GroupName = isMain ? "primaryimage" : "images",
                AssetId = image.Src
            };

            return retVal;
        }

        public VirtoCategory Convert(ShopifyCustomCollection category)
        {
            var result = new VirtoCategory();
            result.ShopifyId = category.Id;
            result.Code = category.Handle;
            result.Name = category.Title;
            result.VirtoId = Guid.NewGuid().ToString();

            return result;
        }
    }
}