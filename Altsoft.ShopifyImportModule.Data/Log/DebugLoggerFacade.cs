using System.Diagnostics;
using Altsoft.ShopifyImportModule.Data.Interfaces;

namespace Altsoft.ShopifyImportModule.Data.Log
{
    public class DebugLoggerFacade:ILoggerFacade
    {
        public void Log(string message, Category logCategory, Priority priority)
        {
            Debug.WriteLine("Logger: message:'{0}', category:'{1}', priority:'{2}'",message,logCategory,priority);
        }
    }
}