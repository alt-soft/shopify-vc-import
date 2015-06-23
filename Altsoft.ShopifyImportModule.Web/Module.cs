using Altsoft.ShopifyImportModule.Data.Converters;
using Altsoft.ShopifyImportModule.Data.Interfaces;
using Altsoft.ShopifyImportModule.Data.Repositories;
using Altsoft.ShopifyImportModule.Data.Services;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Modularity;

namespace Altsoft.ShopifyImportModule.Web
{
    public class Module:IModule
    {
        private readonly IUnityContainer _container;
        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
        }

        public void Initialize()
        {
            _container.RegisterType<IShopifyRepository, ShopifyRepository>();
            _container.RegisterType<IShopifyImportService, ShopifyImportService>();
            _container.RegisterType<IShopifyConverter, ShopifyConverter>();
            _container.RegisterType<IShopifyAuthenticationService, ShopifyAuthenticationService>();
        }

        public void PostInitialize()
        {
        }
    }
}