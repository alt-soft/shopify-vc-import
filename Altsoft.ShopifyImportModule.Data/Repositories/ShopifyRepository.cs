using System;
using System.Collections.Generic;
using System.Net;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using Newtonsoft.Json;

namespace Altsoft.ShopifyImportModule.Data.Repositories
{
    public class ShopifyRepository:IShopifyRepository
    {
        private readonly IShopifyAuthenticationService _shopifyAuthenticationService;
        private readonly ILoggerFacade _loggerFacade;
        public ShopifyRepository(ILoggerFacade loggerFacade, IShopifyAuthenticationService shopifyAuthenticationService)
        {
            _loggerFacade = loggerFacade;
            _shopifyAuthenticationService = shopifyAuthenticationService;
        }

        public IEnumerable<ShopifyProduct> GetShopifyProductsFromSource(ShopifyProductSearchCriteria searchCriteria)
        {
            try
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
            catch (ArgumentException e)
            {
                _loggerFacade.Log(string.Format("Error in getting Shopify products: {0}", e), LogCategory.Exception,
                    LogPriority.High);
                return null;
            }
        }

       

        private string GetRequestUrl(string param)
        {
            var shopName = _shopifyAuthenticationService.GetShopName();
            
            if (string.IsNullOrWhiteSpace(shopName))
                throw new ArgumentException("Shop name is empty!");

            var initialUrl = string.Format("https://{0}.myshopify.com/admin/{1}",  shopName, param);
            return initialUrl;
        }

        public IEnumerable<ShopifyCustomCollection> GetShopifyCollections()
        {

            try
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
            catch (ArgumentException e)
            {
                _loggerFacade.Log(string.Format("Error in getting Shopify Collections: {0}", e), LogCategory.Exception,
                    LogPriority.High);

                return null;
            }
        }

        public IEnumerable<ShopifyCollect> GetShopifyCollects()
        {
            try
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
            catch (ArgumentException e)
            {
                _loggerFacade.Log(string.Format("Error in getting Shopify Collects: {0}", e), LogCategory.Exception,
                     LogPriority.High);

                return null;
            }
        }
    }
}