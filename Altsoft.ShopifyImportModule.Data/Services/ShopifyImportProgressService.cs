using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;

namespace Altsoft.ShopifyImportModule.Data.Services
{
    public class ShopifyImportProgressService:IShopifyImportProgressService
    {
        #region Private Fields

        private ShopifyImportProgress _currentProgress;

        #endregion

        #region IMagentoImportProgressService Implementation

        public ShopifyImportProgressResult GetCurrentProgress()
        {
            return new ShopifyImportProgressResult
            {
                IsSuccess = true,
                ShopifyImportProgress = _currentProgress
            };
        }

        public void StoreCurrentProgress(ShopifyImportProgress currentProgress)
        {
            _currentProgress = currentProgress;
        }

        #endregion
    }
}