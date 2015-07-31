using System;

namespace MySP2010Utilities
{
    public interface ILogUtility
    {
        void TraceDebugException(string ErrorMessage, Type objType, Exception exception);
        void TraceDebugInformation(string MethodMessage, Type objType);
    }
}
