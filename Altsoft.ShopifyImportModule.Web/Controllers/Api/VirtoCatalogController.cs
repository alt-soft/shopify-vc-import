using System;
using System.Web.Http;
using System.Web.Http.Description;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using VirtoCommerce.Domain.Catalog.Model;
using Category = Altsoft.ShopifyImportModule.Data.Interfaces.Category;

namespace Altsoft.ShopifyImportModule.Web.Controllers.Api
{
    [RoutePrefix("api/virtoCatalog")]
    public class VirtoCatalogController:ApiController
    {
        #region Private Fields

        private readonly IVirtoCatalogService _virtoCatalogService;
        private readonly ILoggerFacade _loggerFacade;

        #endregion

        #region Constructors

        public VirtoCatalogController(IVirtoCatalogService virtoCatalogService, ILoggerFacade loggerFacade)
        {
            _virtoCatalogService = virtoCatalogService;
            _loggerFacade = loggerFacade;
        }

        #endregion

        #region API

        [HttpGet]
        [ResponseType(typeof(PaginationResult<Catalog>))]
        [Route("get-catalogs")]
        public IHttpActionResult GetCatalogs()
        {
            try
            {
                var result = _virtoCatalogService.GetCatalogs();
                return Ok(result);
            }
            catch (Exception e)
            {
                _loggerFacade.Log(e.Message + e.StackTrace, Category.Exception, Priority.High);
                return InternalServerError(e);
            }
        }

        [HttpGet]
        [ResponseType(typeof(PaginationResult<VirtoCommerce.Domain.Catalog.Model.Category>))]
        [Route("get-categories")]
        public IHttpActionResult GetCategories([FromUri]VirtoCategorySearchCriteria virtoCategorySearchCriteria)
        {
            try
            {
                var result = _virtoCatalogService.GetCategories(virtoCategorySearchCriteria);
                return Ok(result);
            }
            catch (Exception e)
            {
                _loggerFacade.Log(e.Message + e.StackTrace, Category.Exception, Priority.High);
                return InternalServerError(e);
            }
        }

        #endregion
    }
}