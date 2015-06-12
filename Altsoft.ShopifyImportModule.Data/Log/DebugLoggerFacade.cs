using System.Diagnostics;
using Altsoft.ShopifyImportModule.Data.Interfaces;

namespace Altsoft.ShopifyImportModule.Data.Log
{
    public class DebugLoggerFacade:ILoggerFacade
    {
        public void Log(string message, LogCategory logCategory, LogPriority logPriority)
        {
            Debug.WriteLine("Logger: message:'{0}', category:'{1}', LogPriority:'{2}'",message,logCategory,logPriority);
        }
    }
}