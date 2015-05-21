using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;
using Altsoft.ShopifyImportModule.Web.Converters;
using Altsoft.ShopifyImportModule.Web.Models;

namespace Altsoft.ShopifyImportModule.Web.Controllers.Api
{
    [RoutePrefix("api/shopifyImport")]
    public class ImportController : ApiController
    {
        private IShopifyRepository _shopifyRepository;

        public ImportController(IShopifyRepository shopifyRepository)
        {
            _shopifyRepository = shopifyRepository;
        }

        [HttpGet]
        [ResponseType(typeof(PaginationResult<ShopifyProduct>))]
        [Route("get")]
        public IHttpActionResult Get()
        {
            var products = _shopifyRepository.GetShopifyProductsFromSource(new ShopifyProductSearchCriteria()
            {

            });
            return Ok(products);
        }

        [HttpGet]
        [ResponseType(typeof(PaginationResult<ShopifyProductItem>))]
        [Route("get-collections")]
        public IHttpActionResult GetCollections()
        {
            var products = _shopifyRepository.GetShopifyProductsFromSource(new ShopifyProductSearchCriteria()
            {

            });

            var collections = _shopifyRepository.GetShopifyCollections();
            var collects = _shopifyRepository.GetShopifyCollects();

            var converter = new ShopifyProductItemConverter();
            var result = collections.Items.Select(collection => converter.Convert(collection,collects.Items,products.Items));

            return Ok(result);
        }

        [HttpPost]
        [Route("startimport")]
        public IHttpActionResult StartImport(string parameter)
        {
            return Ok(new { status = "started with parameter" + parameter });
        }
    }
}