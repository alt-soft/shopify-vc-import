namespace Altsoft.ShopifyImportModule.Web.Models
{
    public class ShopifyImportParams
    {
        public string VirtoCatalogId { get; set; }

        public bool ImportProducts { get; set; }

        public bool ImportCollections { get; set; }

        public bool ImportCustomers { get; set; }

        public bool ImportOrders { get; set; }

        public bool ImportThemes { get; set; }

        public string StoreId { get; set; }
    }
}