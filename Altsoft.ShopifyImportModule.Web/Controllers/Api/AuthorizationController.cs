using System.Web.Http;
using System.Web.Http.Description;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;

namespace Altsoft.ShopifyImportModule.Web.Controllers.Api
{
    [RoutePrefix("api/shopifyAuthorization")]
    public class AuthorizationController : ApiController
    {
        private IShopifyAuthorizationService shopifyAuthorizationService;

        public AuthorizationController(IShopifyAuthorizationService shopifyAuthorizationService)
        {
            this.shopifyAuthorizationService = shopifyAuthorizationService;
        }

        [HttpGet]
        [ResponseType(typeof(bool))]
        [Route("isAuthorized")]
        public IHttpActionResult IsAuthorized()
        {
            var isAuthorized = shopifyAuthorizationService.IsAuthorized();
            return Ok(new { isAuthorized });
        }

        [HttpPost]
        [ResponseType(typeof (bool))]
        [Route("authorize")]
        public IHttpActionResult Authorize(LoginModel loginModel)
        {
            var response = shopifyAuthorizationService.Authorize(loginModel);
            return Ok(response);
        }
    }
}