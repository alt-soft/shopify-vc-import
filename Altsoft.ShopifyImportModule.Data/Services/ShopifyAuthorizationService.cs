using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Models;
using VirtoCommerce.Platform.Core.Settings;

namespace Altsoft.ShopifyImportModule.Data.Services
{
    public class ShopifyAuthorizationService:IShopifyAuthorizationService
    {
        private ISettingsManager settingsManager;

        public ShopifyAuthorizationService(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public bool IsAuthorized()
        {
            var accessToken = settingsManager.GetValue<string>("Altsoft.ShopifyImport.Credentials.AccessToken",
                null);

            return !string.IsNullOrWhiteSpace(accessToken);
        }

        public ServiceResponseBase Authorize(LoginModel loginModel)
        {

            //Save access token in settings
            var accessToken = string.Empty;
            settingsManager.SetValue("Altsoft.ShopifyImport.Credentials.AccessToken",accessToken);

            return new ServiceResponseBase()
            {
                IsSuccess = true
            };
        }
    }
}