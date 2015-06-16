using System;
using System.Web.Http;
using System.Web.Http.Description;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;

namespace Altsoft.ShopifyImportModule.Web.Controllers.Api
{
    [RoutePrefix("api/shopifyImport")]
    public class ShopifyImportController : ApiController
    {
        private readonly IShopifyImportService _shopifyImportService;
        private readonly IShopifyImportProgressService _shopifyImportProgressService;
        private readonly ILoggerFacade _loggerFacade;
        public ShopifyImportController(IShopifyImportService shopifyImportService, ILoggerFacade loggerFacade, IShopifyImportProgressService shopifyImportProgressService)
        {
            _shopifyImportService = shopifyImportService;
            _loggerFacade = loggerFacade;
            _shopifyImportProgressService = shopifyImportProgressService;
        }

        [HttpPost]
        [ResponseType(typeof(ServiceResponseBase))]
        [Route("start-import")]
        public IHttpActionResult StartImport(ShopifyImportParams importParams)
        {
            try
            {
                var result = _shopifyImportService.Import(importParams);
                return Ok(result);
            }
            catch (Exception e)
            {
                _loggerFacade.Log(e.Message + e.StackTrace, LogCategory.Exception, LogPriority.High);
                return InternalServerError(e);
            }
        }

        [HttpGet]
        [ResponseType(typeof(ShopifyImportProgressResult))]
        [Route("get-progress")]
        public IHttpActionResult GetProgress()
        {
            try
            {
                var result = _shopifyImportProgressService.GetCurrentProgress();
                return Ok(result);
            }
            catch (Exception e)
            {
                _loggerFacade.Log(e.Message + e.StackTrace, LogCategory.Exception, LogPriority.High);
                return InternalServerError(e);
            }
        }
    }
}