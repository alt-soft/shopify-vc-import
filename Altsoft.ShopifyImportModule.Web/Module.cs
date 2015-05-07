using Altsoft.ShopifyImportModule.Data.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

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
        }

        public void PostInitialize()
        {
        }
    }
}