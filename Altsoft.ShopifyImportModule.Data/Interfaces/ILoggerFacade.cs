namespace Altsoft.ShopifyImportModule.Data.Interfaces
{
    public enum LogCategory
    {
        Exception
    }

    public enum LogPriority
    {
        High,
        Low
    }
    public interface ILoggerFacade
    {
        void Log(string message, LogCategory logCategory, LogPriority logPriority);
    }
}