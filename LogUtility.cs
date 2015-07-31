using System;

namespace MySP2010Utilities
{
    class LogUtility : ILogUtility
    {
        public void TraceDebugException(string ErrorMessage, Type objType, Exception exception)
        {
            SharePointUtilities.TraceDebugException(ErrorMessage, objType, exception);
        }

        public void TraceDebugInformation(string MethodMessage, Type objType)
        {
            SharePointUtilities.TraceDebugInformation(MethodMessage, objType);
        }
    }
}
