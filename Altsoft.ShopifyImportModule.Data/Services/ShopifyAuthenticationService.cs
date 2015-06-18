using System;
using System.Net;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using VirtoCommerce.Platform.Core.Settings;

namespace Altsoft.ShopifyImportModule.Data.Services
{
    public class ShopifyAuthenticationService:IShopifyAuthenticationService
    {
        private const string ApiKeyKeyName = "Altsoft.ShopifyImport.Credentials.APIKey";
        private const string PasswordKeyName = "Altsoft.ShopifyImport.Credentials.Password";
        private const string ShopNameKeyName = "Altsoft.ShopifyImport.Credentials.ShopName";

        private readonly ISettingsManager _settingsManager;

        public ShopifyAuthenticationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }


        public ICredentials GetCridentials()
        {
            var apiKey = _settingsManager.GetValue(ApiKeyKeyName, string.Empty);
            var password = _settingsManager.GetValue(PasswordKeyName, string.Empty);

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("Api key is empty!");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is empty!");

            var result = new NetworkCredential(apiKey, password);

            return result;
        }

        public string GetShopName()
        {
            var result = _settingsManager.GetValue(ShopNameKeyName, string.Empty);

            return result;
        }

        public bool IsAuthenticated()
        {
            var apiKey = _settingsManager.GetValue(ApiKeyKeyName, string.Empty);
            var password = _settingsManager.GetValue(PasswordKeyName, string.Empty);
            var shopName = _settingsManager.GetValue(ShopNameKeyName, string.Empty);

            var isAuthenticated = !string.IsNullOrWhiteSpace(apiKey) &&
                                  !string.IsNullOrWhiteSpace(password) &&
                                  !string.IsNullOrWhiteSpace(shopName);

            return isAuthenticated;
        }

        public void Authenticate(string apiKey, string password, string shopName)
        {
            _settingsManager.SetValue(ApiKeyKeyName, apiKey);
            _settingsManager.SetValue(PasswordKeyName, password);
            _settingsManager.SetValue(shopName, shopName);
        }
    }
}