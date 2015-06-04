using System.Collections.Generic;
using System.Linq;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;

namespace Altsoft.ShopifyImportModule.Test.ServicesTests
{
    [TestClass]
    public class VirtoCatalogServiceTest
    {
        [TestMethod]
        public void GetCatalogTest()
        {
            var catalogService = new Mock<ICatalogService>();

            catalogService.Setup(s => s.GetCatalogsList()).Returns(() => new List<Catalog>()
            {
                new Catalog()
            });

            var searchService = new Mock<ICatalogSearchService>();
            var virtoCatalogService = new VirtoCatalogService(null, catalogService.Object, searchService.Object);

            var catalogs = virtoCatalogService.GetCatalogs();

            Assert.IsNotNull(catalogs);
            Assert.IsTrue(catalogs.IsSuccess);
            Assert.IsNotNull(catalogs.Items);
            Assert.IsTrue(catalogs.Items.Any());
        }

        [TestMethod]
        public void GetCategoriesTest()
        {
            var catalogService = new Mock<ICatalogService>();
            var searchService = new Mock<ICatalogSearchService>();

            searchService.Setup(s => s.Search(It.IsAny<SearchCriteria>())).Returns(() => new SearchResult()
            {
                Categories = new List<Category>()
                {
                    new Category()
                }
            });

            var virtoCatalogService = new VirtoCatalogService(null, catalogService.Object, searchService.Object);

            var categories = virtoCatalogService.GetCategories(new VirtoCategorySearchCriteria(){CatalogId = "123"});

            Assert.IsNotNull(categories);
            Assert.IsTrue(categories.IsSuccess);
            Assert.IsNotNull(categories.Items);
            Assert.IsTrue(categories.Items.Any());
        }
    }
}