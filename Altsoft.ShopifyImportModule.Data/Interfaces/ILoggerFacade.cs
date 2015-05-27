namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public enum Category
    {
        Exception
    }

    public enum Priority
    {
        High,
        Low
    }
    public interface ILoggerFacade
    {
        void Log(string message, Category logCategory, Priority priority);
    }
}