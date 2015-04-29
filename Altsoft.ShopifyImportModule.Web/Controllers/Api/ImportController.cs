using System.Web.Http;

namespace Altsoft.ShopifyImportModule.Web.Controllers.Api
{
     [RoutePrefix("api/shopifyImport")]
    public class ImportController : ApiController
    {
        // GET: api/module1/
        [HttpGet]
         [Route("get")]
        public IHttpActionResult Get()
        {
            return Ok(new[] { "Hello world!" });
        }

        [HttpPost]
        [Route("startimport")]
        public IHttpActionResult StartImport(string parameter)
        {
            return Ok(new {status = "started with parameter" + parameter});
        }
    }
}