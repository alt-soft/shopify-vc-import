using Altsoft.ShopifyImportModule.Data.Models;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IShopifyImportService
    {
        ShopifyImportNotification Import(ShopifyImportParams importParams, ShopifyImportNotification notification);
    }
}