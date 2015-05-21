using System.Collections.Generic;

namespace Altsoft.ShopifyImportModule.Web.Models
{
    //Can be either product or collection
    public class ShopifyProductItem
    {
        public string Title { get; set; }
        public string BodyHtml { get; set; }
        public string Image { get; set; }

        public bool IsCollection { get; set; }
        public List<ShopifyProductItem> Children{ get; set; }

        public bool? IsSelected { get; set; }

        public long Id { get; set; }
    }
}