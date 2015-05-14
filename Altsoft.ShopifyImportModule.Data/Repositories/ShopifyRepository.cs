using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Settings;

namespace Altsoft.ShopifyImportModule.Data.Repositories
{
    public class ShopifyRepository:IShopifyRepository
    {
        private ISettingsManager _settingsManager;

        public ShopifyRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public PaginationResult<ShopifyProduct> GetShopifyProductsFromSource(ShopifyProductSearchCriteria searchCriteria)
        {
            try
            {
                var requestUrl = GetRequestUrl("products.json");
                var cridentials = GetCridentials();
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Credentials = cridentials;

                    var json = webClient.DownloadString(requestUrl);
                    var shopifyProductList = JsonConvert.DeserializeObject<ShopifyProductList>(json);

                    return new PaginationResult<ShopifyProduct>()
                    {
                        Items = shopifyProductList.Products,
                        TotalCount = shopifyProductList.Products.Length,
                        IsSuccess = true
                    };
                }
            }
            catch (ArgumentException e)
            {
                return new PaginationResult<ShopifyProduct>()
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
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

        public PaginationResult<ShopifyCustomCollection> GetShopifyCollections()
        {

            try
            {
                var requestUrl = GetRequestUrl("custom_collections.json");
                var cridentials = GetCridentials();
                using (var webClient = new WebClient())
                {
                    webClient.Credentials = cridentials;

                    var json = webClient.DownloadString(requestUrl);
                    var shopifyProductList = JsonConvert.DeserializeObject<ShopifyCustomCollectionList>(json);

                    return new PaginationResult<ShopifyCustomCollection>()
                    {
                        Items = shopifyProductList.CustomCollections,
                        TotalCount = shopifyProductList.CustomCollections.Length,
                        IsSuccess = true
                    };
                }
            }
            catch (ArgumentException e)
            {
                return new PaginationResult<ShopifyCustomCollection>()
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}