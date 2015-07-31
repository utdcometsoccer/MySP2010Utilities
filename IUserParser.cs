using System;
namespace MySP2010Utilities
{
    public interface IUserParser
    {
        string parseUser(string userValue, Microsoft.SharePoint.SPSite site);
        string parseUsers(string userValue, char separator, Microsoft.SharePoint.SPSite site);
    }
}
