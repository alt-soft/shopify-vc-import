using System;
using System.Collections.Generic;
using System.Net;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using Newtonsoft.Json;

namespace Altsoft.ShopifyImportModule.Data.Repositories
{
    public class ShopifyRepository : IShopifyRepository
    {
        private readonly IShopifyAuthenticationService _shopifyAuthenticationService;

        public ShopifyRepository(
            IShopifyAuthenticationService shopifyAuthenticationService)
        {
            _shopifyAuthenticationService = shopifyAuthenticationService;
        }

        public IEnumerable<ShopifyProduct> GetShopifyProducts()
        {
            var requestUrl = GetRequestUrl("products.json");
            var cridentials = _shopifyAuthenticationService.GetCridentials();
            using (var webClient = new WebClient())
            {
                webClient.Credentials = cridentials;

                var json = webClient.DownloadString(requestUrl);
                var result = JsonConvert.DeserializeObject<ShopifyProductList>(json);

                return result.Products;
            }
        }

        private string GetRequestUrl(string param)
        {
            var shopName = _shopifyAuthenticationService.GetShopName();

            if (string.IsNullOrWhiteSpace(shopName))
                throw new ArgumentException("Shop name is empty!");

            var initialUrl = string.Format("https://{0}.myshopify.com/admin/{1}", shopName, param);
            return initialUrl;
        }

        public IEnumerable<ShopifyCustomCollection> GetShopifyCollections()
        {
            var requestUrl = GetRequestUrl("custom_collections.json");
            var cridentials = _shopifyAuthenticationService.GetCridentials();
            using (var webClient = new WebClient())
            {
                webClient.Credentials = cridentials;

                var json = webClient.DownloadString(requestUrl);
                var result = JsonConvert.DeserializeObject<ShopifyCustomCollectionList>(json);

                return result.CustomCollections;
            }
        }

        public IEnumerable<ShopifyCollect> GetShopifyCollects()
        {

            var requestUrl = GetRequestUrl("collects.json");
            var cridentials = _shopifyAuthenticationService.GetCridentials();
            using (var webClient = new WebClient())
            {
                webClient.Credentials = cridentials;

                var json = webClient.DownloadString(requestUrl);
                var result = JsonConvert.DeserializeObject<ShopifyCollectList>(json);

                return result.Collects;
            }
        }
    }
}