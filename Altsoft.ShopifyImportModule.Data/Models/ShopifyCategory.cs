using System.Collections.Generic;

namespace Altsoft.ShopifyImportModule.Data.Models
{
    public class ShopifyCategory
    {
         #region Constructors

        public ShopifyCategory()
        {
            ChildCategories = new List<ShopifyCategory>();
        }

        #endregion

        #region Public Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public List<ShopifyCategory> ChildCategories { get; private set; }

        #endregion 
    }
}