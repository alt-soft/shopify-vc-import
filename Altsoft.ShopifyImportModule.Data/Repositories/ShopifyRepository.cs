using System;
using System.Collections.Generic;
using System.Net;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Settings;

namespace Altsoft.ShopifyImportModule.Data.Repositories
{
    public class ShopifyRepository:IShopifyRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly ILoggerFacade _loggerFacade;
        public ShopifyRepository(ISettingsManager settingsManager, ILoggerFacade loggerFacade)
        {
            _settingsManager = settingsManager;
            _loggerFacade = loggerFacade;
        }

        public IEnumerable<ShopifyProduct> GetShopifyProductsFromSource(ShopifyProductSearchCriteria searchCriteria)
        {
            try
            {
                var requestUrl = GetRequestUrl("products.json");
                var cridentials = GetCridentials();
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
                _loggerFacade.Log(string.Format("Error in getting Shopify products: {0}", e), Category.Exception,
                    Priority.High);
                return null;
            }
        }

        private ICredentials GetCridentials()
        {
            var apiKey = _settingsManager.GetValue("Altsoft.ShopifyImport.Credentials.APIKey", string.Empty);
            var password = _settingsManager.GetValue("Altsoft.ShopifyImport.Credentials.Password", string.Empty);

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("Api key is empty!");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is empty!");

            var result = new NetworkCredential(apiKey, password);

            return result;
        }

        private string GetRequestUrl(string param)
        {
            var shopName = _settingsManager.GetValue("Altsoft.ShopifyImport.Credentials.ShopName", string.Empty);
            
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
                var cridentials = GetCridentials();
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
                _loggerFacade.Log(string.Format("Error in getting Shopify Collections: {0}", e), Category.Exception,
                    Priority.High);

                return null;
            }
        }

        public IEnumerable<ShopifyCollect> GetShopifyCollects()
        {
            try
            {
                var requestUrl = GetRequestUrl("collects.json");
                var cridentials = GetCridentials();
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
                _loggerFacade.Log(string.Format("Error in getting Shopify Collects: {0}", e), Category.Exception,
                     Priority.High);

                return null;
            }
        }
    }
}