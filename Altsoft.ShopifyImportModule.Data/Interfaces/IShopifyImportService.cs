using Altsoft.ShopifyImportModule.Data.Models;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IShopifyImportService
    {
        ServiceResponseBase Import(ShopifyImportParams importParams);
    }
}