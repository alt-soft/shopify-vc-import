using Altsoft.ShopifyImportModule.Data.Models;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IShopifyImportProgressService
    {
        void StoreCurrentProgress(ShopifyImportProgress currentProgress);
        ShopifyImportProgressResult GetCurrentProgress();
    }
}