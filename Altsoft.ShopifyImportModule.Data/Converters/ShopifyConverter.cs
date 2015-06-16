using System;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using VirtoCommerce.CatalogModule.Data.Model;

namespace Altsoft.ShopifyImportModule.Data.Converters
{
    public class ShopifyConverter:IShopifyConverter
    {
        public Product Convert(ShopifyProduct product)
        {
            var result = new Product();
            result.Id = product.Id.ToString();
            result.Name = product.Title;
            result.Code = product.Handle;
            result.CreatedDate = DateTime.Now;
            result.StartDate = product.PublishedAt;

            return result;
        }

        public VirtoCategory Convert(ShopifyCustomCollection category)
        {
            var result = new VirtoCategory();
            result.ShopifyId = category.Id;
            result.Code = category.Handle;
            result.Name = category.Title;
            result.VirtoId = category.Handle;

            return result;
        }
    }
}