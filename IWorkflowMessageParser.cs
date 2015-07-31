using System;
namespace MySP2010Utilities
{
    public interface IWorkflowMessageParser
    {
        string parseMessage(string message, Microsoft.SharePoint.SPListItem item);
    }
}
