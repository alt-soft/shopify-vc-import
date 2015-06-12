using System.Collections.Generic;

namespace Altsoft.ShopifyImportModule.Data.Models
{
    public class ShopifyImportParams
    {
        public IEnumerable<long> ShopifyProductIds { get; set; }

        public string VirtoCatalogId { get; set; }

        public string VirtoCategoryId { get; set; }

        public bool IsRetainCategoryHierarchy { get; set; }
    }
}