using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Platform.Core.Common;

namespace Altsoft.ShopifyImportModule.Data.Converters
{
    public class ShopifyConverter : IShopifyConverter
    {
        private ItemAsset Convert(ShopifyImage image, bool isMain)
        {
            var retVal = new ItemAsset
            {
                CreatedDate = image.CreatedAt,
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
                CatalogId = importParams.VirtoCatalogId
            };

            return result;
        }

        public CatalogProduct Convert(ShopifyProduct shopifyProduct, ShopifyImportParams importParams, ShopifyData shopifyData)
        {

            var retVal = new CatalogProduct
            {
                Name = shopifyProduct.Title,
                Code = shopifyProduct.Handle,
                StartDate = shopifyProduct.PublishedAt,
                IsActive = true,
                CatalogId = importParams.VirtoCatalogId,
                ProductType = shopifyProduct.ProductType,
                Vendor = shopifyProduct.Vendor
            };

            //Images
            if (shopifyProduct.Image != null)
            {
                retVal.Assets = new List<ItemAsset>();
                retVal.Assets.Add(Convert(shopifyProduct.Image, true));
            }

            //Review
            if (shopifyProduct.BodyHtml != null)
            {
                retVal.Reviews = new List<EditorialReview>();
                var review = new EditorialReview
                {
                    Content = shopifyProduct.BodyHtml,
                    LanguageCode = "en-us",
                };
                retVal.Reviews.Add(review);
            }

            //Seo
            retVal.SeoInfos = new List<SeoInfo>();
            var seoInfo = new SeoInfo
            {
                SemanticUrl = shopifyProduct.Title.GenerateSlug(),
                LanguageCode = "en-us"
            };
            retVal.SeoInfos.Add(seoInfo);


            //TODO: Inventory

            //Variation
            if (shopifyProduct.Variants != null)
            {
                retVal.Variations = new List<CatalogProduct>();
                var isFirst = true;
                foreach (var shopifyVariant in shopifyProduct.Variants)
                {
                    var variation = isFirst ? retVal : new CatalogProduct();
                    variation.Name = String.Format("{0} ({1})", retVal.Name, shopifyVariant.Title);
                    variation.Code = (shopifyProduct.Handle + "-" + shopifyVariant.Title).GenerateSlug();
                    variation.IsActive = true;

                    variation.SeoInfos = new List<SeoInfo>();
                    seoInfo = new SeoInfo
                    {
                        SemanticUrl = variation.Name.GenerateSlug(),
                        LanguageCode = "en-us"
                    };
                    retVal.SeoInfos.Add(seoInfo);

                    if (shopifyVariant.ImageId != null && shopifyProduct.Images != null)
                    {
                        variation.Assets = new List<ItemAsset>();
                        var image = shopifyProduct.Images.Where(x => x.Id == shopifyVariant.ImageId).Select(x => Convert(x, true)).FirstOrDefault();
                        if (image != null)
                        {
                            variation.Assets.Add(image);
                        }
                    }

                    //Price
                    variation.Prices = new List<Price>();
                    variation.Prices.Add(new Price
                    {
                        Sale = shopifyVariant.Price,
                        List = shopifyVariant.Price,
                        Currency = CurrencyCodes.USD
                    });


                    //Properties (need refactor)
                    variation.PropertyValues = new List<PropertyValue>();
                    if (shopifyVariant.Option1 != null)
                    {
                        var propValue = new PropertyValue
                        {
                            PropertyName = shopifyProduct.Options[0].Name,
                            Value = shopifyVariant.Option1,
                            ValueType = PropertyValueType.ShortText
                        };
                    }
                    if (shopifyVariant.Option2 != null)
                    {
                        var propValue = new PropertyValue
                        {
                            PropertyName = shopifyProduct.Options[1].Name,
                            Value = shopifyVariant.Option2,
                            ValueType = PropertyValueType.ShortText
                        };
                    }
                    if (shopifyVariant.Option3 != null)
                    {
                        var propValue = new PropertyValue
                        {
                            PropertyName = shopifyProduct.Options[2].Name,
                            Value = shopifyVariant.Option3,
                            ValueType = PropertyValueType.ShortText
                        };
                    }

                    if (!isFirst)
                    {
                        retVal.Variations.Add(variation);
                    }
                    isFirst = false;
                }
            }


            if (importParams.ImportCollections)
            {
                var firstCollect = shopifyData.Collects.FirstOrDefault(collect => collect.ProductId == shopifyProduct.Id);
                if (firstCollect != null)
                {
                    retVal.Category = new Category()
                    {
                        Code =
                            shopifyData.Collections.First(collection => collection.Id == firstCollect.CollectionId)
                                .Handle
                    };
                }
            }

            return retVal;
        }
    }
}