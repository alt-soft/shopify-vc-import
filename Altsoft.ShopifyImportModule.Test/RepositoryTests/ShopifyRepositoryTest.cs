using System.Linq;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using Altsoft.ShopifyImportModule.Data.Repositories;
using Altsoft.ShopifyImportModule.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VirtoCommerce.Platform.Core.Settings;

namespace Altsoft.ShopifyImportModule.Test.RepositoryTests
{
    [TestClass]
    public class ShopifyRepositoryTest
    {
        [TestMethod]
        public void GetShopifyCollectsTest()
        {
            var shopifyAuthenticationService = GetAuthService();

            var repository = new ShopifyRepository(shopifyAuthenticationService);

            var collects = repository.GetShopifyCollects();

            Assert.IsNotNull(collects);
            Assert.IsTrue(collects.Any());
        }

       
        [TestMethod]
        public void GetShopifyCollectionsTest()
        {
            var shopifyAuthenticationService = GetAuthService();

            var repository = new ShopifyRepository(shopifyAuthenticationService);

            var collections = repository.GetShopifyCollections();

            Assert.IsNotNull(collections);
            Assert.IsTrue(collections.Any());
        }

      

        [TestMethod]
        public void GetShopifyProductsFromSource()
        {
            var shopifyAuthenticationService = GetAuthService();

            var repository = new ShopifyRepository(shopifyAuthenticationService);

            var products = repository.GetShopifyProducts();

            Assert.IsNotNull(products);
            Assert.IsTrue(products.Any());

        }
        private IShopifyAuthenticationService GetAuthService()
        {
            var settingsManagerMock = GetSettingsServiceMock();
            var settingsManager = settingsManagerMock.Object;
            var shopifyAuthenticationService = new ShopifyAuthenticationService(settingsManager);

            return shopifyAuthenticationService;
        }


        private static Mock<ISettingsManager> GetSettingsServiceMock()
        {
            var settingsServiceMock = new Mock<ISettingsManager>();
            settingsServiceMock.Setup(m => m.GetValue("Altsoft.ShopifyImport.Credentials.APIKey", string.Empty))
                .Returns("d7a2d3db100f5ce3c0e19c8831aaa9f1");

            settingsServiceMock.Setup(m => m.GetValue("Altsoft.ShopifyImport.Credentials.ShopName", string.Empty))
                .Returns("shopify-import-test-shop");

            settingsServiceMock.Setup(m => m.GetValue("Altsoft.ShopifyImport.Credentials.Password", string.Empty))
                .Returns("c64bbf7cd5d3a4cda8f52efe10e82992");
            return settingsServiceMock;
        }
    }
}
