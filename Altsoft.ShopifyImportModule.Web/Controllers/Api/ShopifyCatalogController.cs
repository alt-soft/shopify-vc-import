using System.Web.Http;
using System.Web.Http.Description;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Data.Models.Shopify;

namespace Altsoft.ShopifyImportModule.Web.Controllers.Api
{
    [RoutePrefix("api/shopifyCatalog")]
    public class ShopifyCatalogController:ApiController
    {
        private readonly IShopifyService _shopifyService;

        public ShopifyCatalogController(IShopifyService shopifyService)
        {
            _shopifyService = shopifyService;
        }


        [HttpGet]
        [ResponseType(typeof(PaginationResult<ShopifyProductItem>))]
        [Route("get-collections")]
        public IHttpActionResult GetCollections()
        {
            var result = _shopifyService.GetShopifyCollections();

            return Ok(result);
        }
    }
}