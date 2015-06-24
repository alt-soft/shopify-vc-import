using System.Web.Http;
using System.Web.Http.Description;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Web.Models;

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
        public IHttpActionResult Authenticate(AuthenticationModel model)
        {
            _shopifyAuthenticationService.Authenticate(model.ApiKey, model.Password, model.ShopName);

            return Ok();
        }
    }
}