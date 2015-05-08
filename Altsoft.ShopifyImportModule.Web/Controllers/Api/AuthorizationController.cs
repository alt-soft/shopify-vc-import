﻿using System.Web.Http;
using System.Web.Http.Description;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;

namespace Altsoft.ShopifyImportModule.Web.Controllers.Api
{
    [RoutePrefix("api/shopifyAuthorization")]
    public class AuthorizationController : ApiController
    {

      

        [HttpGet]
        [ResponseType(typeof(bool))]
        [Route("isAuthorized")]
        public IHttpActionResult IsAuthorized()
        {
            return Ok(new { isAuthorized  = true});
        }

        [HttpPost]
        [ResponseType(typeof (bool))]
        [Route("authorize")]
        public IHttpActionResult Authorize(LoginModel loginModel)
        {
            return Ok(new {response = true});
        }
    }
}
