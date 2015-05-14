using System.Linq;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using Altsoft.ShopifyImportModule.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VirtoCommerce.Platform.Core.Settings;

namespace Altsoft.ShopifyImportModule.Test.RepositoryTests
{
    [TestClass]
    public class ShopifyRepositoryTest
    {
        [TestMethod]
        public void GetShopifyCollectionsTest()
        {
            var settingsServiceMock = GetSettingsServiceMock();

            var settingsManager = settingsServiceMock.Object;
            var repository = new ShopifyRepository(settingsManager);

            var collections = repository.GetShopifyCollections();

            Assert.IsNotNull(collections);
            Assert.IsTrue(collections.IsSuccess);
            Assert.IsNotNull(collections.Items);
            Assert.IsTrue(collections.Items.Any());
        }

      

        [TestMethod]
        public void GetShopifyProductsFromSource()
        {
            var settingsServiceMock = GetSettingsServiceMock();

            var settingsManager = settingsServiceMock.Object;
            var repository = new ShopifyRepository(settingsManager);

            var products = repository.GetShopifyProductsFromSource(new ShopifyProductSearchCriteria());

            Assert.IsNotNull(products);
            Assert.IsTrue(products.IsSuccess);
            Assert.IsNotNull(products.Items);
            Assert.IsTrue(products.Items.Any());

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
