using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
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
                
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString(requestUrl);
                    byte[] byteArray = Encoding.Unicode.GetBytes(json);
                    var stream = new MemoryStream(byteArray);
                    var serializer = new DataContractJsonSerializer(typeof(ShopifyProduct[]));
                    var sdopifyProcProducts = (ShopifyProduct[])serializer.ReadObject(stream);
                    return new PaginationResult<ShopifyProduct>()
                    {
                        Items = sdopifyProcProducts,
                        TotalCount = sdopifyProcProducts.Length
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

        private string GetRequestUrl(string param)
        {
            var shopName = _settingsManager.GetValue("Altsoft.ShopifyImport.Credentials.ShopName", string.Empty);
            var apiKey = _settingsManager.GetValue("Altsoft.ShopifyImport.Credentials.APIKey", string.Empty);
            var password = _settingsManager.GetValue("Altsoft.ShopifyImport.Credentials.Password", string.Empty);

            if (string.IsNullOrWhiteSpace(shopName))
                throw new ArgumentException("Shop name is empty!");

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("Api key is empty!");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is empty!");

            var initialUrl = string.Format("https://{0}:{1}@{2}.myshopify.com/admin/{3}", apiKey, password, shopName, param);
            return initialUrl;
        }

        public IEnumerable<ShopifyCategory> GetShopifyCategoriesTree()
        {
          
            throw new NotImplementedException();
        }
    }
}