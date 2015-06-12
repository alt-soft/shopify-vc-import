using Altsoft.ShopifyImportModule.Data.Models;
using VirtoCommerce.CatalogModule.Data.Model;

namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public interface IVirtoConverter
    {
        Catalog Convert(CatalogBase catalog);

        VirtoCategory Convert(CategoryBase category);

        Product Convert(Product virtoProduct, string catalogId); 
    }
}