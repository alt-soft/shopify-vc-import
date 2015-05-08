using System;
using Altsoft.ShopifyImportModule.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Altsoft.ShopifyImportModule.Test.RepositoryTests
{
    [TestClass]
    public class ShopifyRepositoryTest
    {
        [TestMethod]
        public void GetGetShopifyCategoriesTreeTest()
        {
            var repository = new ShopifyRepository(null);

            var tree = repository.GetShopifyCategoriesTree();

            Assert.IsNotNull(tree);

        }
    }
}
