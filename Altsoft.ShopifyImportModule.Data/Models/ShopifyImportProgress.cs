namespace Altsoft.ShopifyImportModule.Data.Models
{
    public class ShopifyImportProgress
    {
        public int AddedItemsCount { get; set; }

        public int TotalItemsCount { get; set; }

        public int ProgressPercentage
        {
            get { return TotalItemsCount != 0 ? (AddedItemsCount * 100 / TotalItemsCount) : 0; }
        }

        public string CurrentOperationDescription { get; set; }
    }
}