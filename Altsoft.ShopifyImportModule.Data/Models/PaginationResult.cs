using System.Collections.Generic;

namespace Altsoft.ShopifyImportModule.Data.Models
{
    public class PaginationResult<T>:ServiceResponseBase
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}