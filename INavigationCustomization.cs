using Microsoft.SharePoint;

namespace MySP2010Utilities
{
    public interface INavigationCustomization
    {
        void AddPageToNavigation(SPWeb web, SPFile page, string navTitle);
    }
}
