namespace Altsoft.ShopifyImportModule.Data.Models
{
    public class ShopifyImportParams
    {
        public string VirtoCatalogId { get; set; }

        public bool ImportProducts { get; set; }

        public bool ImportImages { get; set; }

        public bool ImportProperties { get; set; }

        public bool ImportCustomers { get; set; }

        public bool ImportOrders { get; set; }

        public bool ImportThemes { get; set; }

        public string StoreId { get; set; }
    }
}