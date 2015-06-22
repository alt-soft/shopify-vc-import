using VirtoCommerce.CatalogModule.Web.Model.Notifications;

namespace Altsoft.ShopifyImportModule.Web.Models
{
    public class ShopifyImportNotification : JobNotificationBase
    {
        public ShopifyImportNotification(string creator)
            : base(creator)
        {
            NotifyType = "CatalogShopifyImport";
        }
    }
}