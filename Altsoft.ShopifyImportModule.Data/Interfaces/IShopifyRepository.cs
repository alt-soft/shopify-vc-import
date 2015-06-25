using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IShopifyRepository
    {
        #region Public Methods

        IEnumerable<ShopifyProduct> GetShopifyProducts();

        IEnumerable<ShopifyCustomCollection> GetShopifyCollections();

        IEnumerable<ShopifyCollect> GetShopifyCollects();

        IEnumerable<ShopifyTheme> GetShopifyThemes();

        IEnumerable<ShopifyAsset> GetShopifyAssets(long themeId);

        ShopifyAsset DownloadShopifyAsset(long themeId, ShopifyAsset asset);

        #endregion
    }
}