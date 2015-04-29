using Altsoft.ShopifyImportModule.Data.Interfaces;

namespace Altsoft.ShopifyImportModule.Data.Services
{
    public class ShopifyAuthorizationService:IShopifyAuthorizationService
    {
        public bool IsAuthorized()
        {
            return true;
        }
    }
}