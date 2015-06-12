namespace Altsoft.ShopifyImportModule.Data.Models
{
    public class VirtoCategory
    {
        public long ShopifyId { get; set; }

        public string VirtoId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int Priority { get; set; }

        public string ParentCategoryId { get; set; }

        public string CatalogId { get; set; } 
    }
}