using Microsoft.SharePoint.Administration;

namespace MySP2010Utilities
{
    class TimerJobOperations : ITimerJobOperations
    {
        public void RemoveTimerJob(SPWebApplication WebApplication, string timerJobName)
        {
            SharePointUtilities.RemoveTimerJob(WebApplication, timerJobName);
        }
    }
}
