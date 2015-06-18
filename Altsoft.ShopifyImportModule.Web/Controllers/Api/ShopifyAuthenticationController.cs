using System.Web.Http;
using Altsoft.ShopifyImportModule.Data.Interfaces;

namespace Alt_soft.ShopifyImportModule.Web.Controllers.Api
{
    [RoutePrefix("api/shopifyAuthentication")]
    public class ShopifyAuthenticationController : ApiController
    {
        private IShopifyAuthenticationService _shopifyAuthenticationService;

        public ShopifyAuthenticationController(IShopifyAuthenticationService shopifyAuthenticationService)
        {
            _shopifyAuthenticationService = shopifyAuthenticationService;
        }

        [HttpGet]
        [Route("is-authenticated")]
        public IHttpActionResult IsAuthenticated()
        {
            var isAuthenticated = _shopifyAuthenticationService.IsAuthenticated();

            return Ok(new { IsAuthenticated = isAuthenticated });
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(string apiKey, string password, string shopName)
        {
            _shopifyAuthenticationService.Authenticate(apiKey, password, shopName);

            return Ok();
        }
    }
}