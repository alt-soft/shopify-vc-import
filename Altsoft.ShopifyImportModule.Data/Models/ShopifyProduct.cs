using System.Runtime.Serialization;

namespace Altsoft.ShopifyImportModule.Data.Models
{
    [DataContract]
    public class ShopifyProduct
    {
        [DataMember(Name="body_html")]
        public string BodyHtml { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }  

    }
}