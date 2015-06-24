using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

        public ZipArchive GetShopifyThemeZip(long themeId)
        {
            //TODO make parallel downloading
            var assets = GetShopifyAssets(themeId);
            var stream = new MemoryStream();
            var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true);

            var cridentials = _shopifyAuthenticationService.GetCridentials();
            using (var webClient = new WebClient())
            {
                webClient.Credentials = cridentials;
                foreach (var asset in assets)
                {
                    var url =
                        GetRequestUrl(string.Format("themes/{0}/assets.json?asset[key]={1}&theme_id={0}", themeId,
                            asset.Key));

                    var json = webClient.DownloadString(url);
                    var downloadedAssetContainer = JsonConvert.DeserializeObject<ShopifyAssetContainer>(json);
                    var downloadedAsset = downloadedAssetContainer.Asset;
                    var entry = zipArchive.CreateEntry(string.Format("{0}/{1}",themeId,asset.Key));
                    using (var entryStream = entry.Open())
                    {
                        if (downloadedAsset.Value != null)
                        {
                            using (var writer = new StreamWriter(entryStream))
                            {
                                writer.Write(downloadedAsset.Value);
                            }
                        }
                        else
                        {
                            if (downloadedAsset.Attachment != null)
                            {
                                var data = Convert.FromBase64String(downloadedAsset.Attachment);
                                entryStream.Write(data, 0, data.Length);
                            }
                        }
                    }

                }
            }

            return zipArchive;
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