using System.Web.Http;
using System.Web.Http.Description;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;

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
            return Ok(new{products});
        }

        [HttpPost]
        [Route("startimport")]
        public IHttpActionResult StartImport(string parameter)
        {
            return Ok(new { status = "started with parameter" + parameter });
        }
    }
}