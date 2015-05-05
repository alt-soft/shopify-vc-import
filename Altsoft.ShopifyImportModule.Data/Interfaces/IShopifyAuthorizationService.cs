using Altsoft.ShopifyImportModule.Data.Models;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IShopifyAuthorizationService
    {
        bool IsAuthorized();
        ServiceResponseBase Authorize(LoginModel loginModel);
    }
}