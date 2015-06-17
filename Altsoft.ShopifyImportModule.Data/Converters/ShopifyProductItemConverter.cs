using System.Collections.Generic;
using System.Linq;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;

namespace Altsoft.ShopifyImportModule.Data.Converters
{
    public class ShopifyProductItemConverter
    {
        public static ShopifyProductItem Convert(ShopifyProduct product)
        {
            var result = new ShopifyProductItem()
            {
                IsCollection = false,
                IsSelected = false,
                IsImported = product.IsImported,

                Title = product.Title,
                BodyHtml = product.BodyHtml,
                Image = product.Image == null ? null : product.Image.Src,
                Id = product.Id
            };

            return result;
        }

        public static ShopifyProductItem Convert(ShopifyCustomCollection collection, IEnumerable<ShopifyCollect> collects, IEnumerable<ShopifyProduct> products)
        {
            var result = new ShopifyProductItem()
            {
                IsCollection = true,
                IsSelected = false,

                Id = collection.Id,
                Title = collection.Title,
                BodyHtml = collection.BodyHtml,
                Image = collection.Image.Src,

                Children = collects
                .Where(collect => collect.CollectionId == collection.Id)
                .Select(collect => Convert(products.First(product => product.Id == collect.ProductId))).ToList()
            };

            result.IsImported = result.Children.All(child => child.IsImported);

            return result;
        }
    }
}