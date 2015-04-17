using System.Web.Http;

namespace Altsoft.ShopifyImportModule.Web.Controllers.Api
{
    public class ShopifyImportController : ApiController
    {
        // GET: api/module1/
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(new[] { "Hello world!" });
        }

        [HttpPost]
        public IHttpActionResult StartImport(string parameter)
        {
            return Ok(new {status = "started with parameter" + parameter});
        }
    }
}