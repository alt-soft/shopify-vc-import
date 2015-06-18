using System.Web.Http;

namespace Alt_soft.ShopifyImportModule.Web.Controllers.Api
{
    [RoutePrefix("api/shopifyAuthentication")]
    public class ShopifyAuthenticationController : ApiController
    {
        [HttpGet]
        [Route("is-authenticated")]
        public IHttpActionResult IsAuthenticated()
        {
            var result = new {IsAuthenticated = true};

            return Ok(result);
        }

        [HttpGet]
        [Route("authenticate")]
        public IHttpActionResult Authenticate()
        {
            return Ok();
        }
    }
}