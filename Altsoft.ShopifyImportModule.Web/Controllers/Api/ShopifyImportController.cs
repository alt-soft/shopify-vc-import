using System;
using System.Web.Http;
using System.Web.Http.Description;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Altsoft.ShopifyImportModule.Web.BackgroundJobs;
using Altsoft.ShopifyImportModule.Web.Models;
using Hangfire;
using VirtoCommerce.CatalogModule.Web.BackgroundJobs;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notification;

namespace Altsoft.ShopifyImportModule.Web.Controllers.Api
{
    [RoutePrefix("api/shopifyImport")]
    public class ShopifyImportController : ApiController
    {
        private readonly INotifier _notifier;
        public ShopifyImportController(INotifier notifier)
        {
            _notifier = notifier;
        }

        [HttpPost]
        [ResponseType(typeof(ServiceResponseBase))]
        [Route("start-import")]
        public IHttpActionResult StartImport(ShopifyImportParams importParams)
        {
            var notification = new ShopifyImportNotification(CurrentPrincipal.GetCurrentUserName())
            {
                Title = "Import catalog from Shopify",
                Description = "starting import...."
            };
            _notifier.Upsert(notification);

            var importJob = new ShopifyCatalogImportJob();
            BackgroundJob.Enqueue(() => importJob.DoImport(importParams, notification));

            return Ok(notification);
        }
    }
}