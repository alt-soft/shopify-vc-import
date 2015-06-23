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
            var result = GetShopifyList<ShopifyProduct,ShopifyProductList>("products.json", list => list.Products);
            return result;
        }

        public IEnumerable<ShopifyCustomCollection> GetShopifyCollections()
        {
            var result = GetShopifyList<ShopifyCustomCollection, ShopifyCustomCollectionList>("custom_collections.json", list => list.CustomCollections);
            return result;
        }

        public IEnumerable<ShopifyCollect> GetShopifyCollects()
        {
            var result = GetShopifyList<ShopifyCollect, ShopifyCollectList>("collects.json", list => list.Collects);
            return result;
        }

        public IEnumerable<ShopifyTheme> GetShopifyThemes()
        {
            var result = GetShopifyList<ShopifyTheme,ShopifyThemeList>("themes.json", list => list.Themes);
            return result;
        }

        public IEnumerable<ShopifyAsset> GetShopifyAssets(long themeId)
        {
            var result = GetShopifyList<ShopifyAsset, ShopifyAssetList>(string.Format("themes/{0}/assets.json",themeId), list => list.Assets);
            return result;
        }


        private IEnumerable<TItem> GetShopifyList<TItem,TList>(string endpoint,Func<TList,IEnumerable<TItem>> getCollection)
        {
            var requestUrl = GetRequestUrl(endpoint);
            var cridentials = _shopifyAuthenticationService.GetCridentials();
            using (var webClient = new WebClient())
            {
                webClient.Credentials = cridentials;

                var json = webClient.DownloadString(requestUrl);
                var result = JsonConvert.DeserializeObject<TList>(json);

                return getCollection(result);
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

       
    }
}