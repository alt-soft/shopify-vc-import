using System.Web.Http;
using System.Web.Http.Description;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using Hangfire;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notification;

namespace Altsoft.ShopifyImportModule.Web.Controllers.Api
{
    [RoutePrefix("api/shopifyImport")]
    public class ShopifyImportController : ApiController
    {
        private readonly INotifier _notifier;
        private readonly IShopifyImportService _shopifyImportService;
        public ShopifyImportController(INotifier notifier, IShopifyImportService shopifyImportService)
        {
            _notifier = notifier;
            _shopifyImportService = shopifyImportService;
        }

        [HttpPost]
        [ResponseType(typeof(ShopifyImportNotification))]
        [Route("start-import")]
        public IHttpActionResult StartImport(ShopifyImportParams importParams)
        {
            var notification = new ShopifyImportNotification(CurrentPrincipal.GetCurrentUserName())
            {
                Title = "Import catalog from Shopify",
                Description = "starting import...."
            };
            _notifier.Upsert(notification);

            BackgroundJob.Enqueue(() => _shopifyImportService.Import(importParams, notification));

            return Ok(notification);
        }
    }
}