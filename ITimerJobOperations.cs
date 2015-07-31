using Microsoft.SharePoint.Administration;

namespace MySP2010Utilities
{
    public interface ITimerJobOperations
    {
        void RemoveTimerJob(SPWebApplication WebApplication, string timerJobName);
    }
}
