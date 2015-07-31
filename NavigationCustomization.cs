using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySP2010Utilities
{
    class NavigationCustomization : INavigationCustomization
    {
        public void AddPageToNavigation(Microsoft.SharePoint.SPWeb web, Microsoft.SharePoint.SPFile page, string navTitle)
        {
            SharePointUtilities.AddPageToNavigation(web, page, navTitle);
        }
    }
}
