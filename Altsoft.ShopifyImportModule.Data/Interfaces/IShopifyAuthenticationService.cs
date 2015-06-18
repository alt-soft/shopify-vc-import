using System.Net;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IShopifyAuthenticationService
    {
        ICredentials GetCridentials();
        string GetShopName();
        bool IsAuthenticated();
        void Authenticate(string apiKey, string password, string shopName);
    }
}