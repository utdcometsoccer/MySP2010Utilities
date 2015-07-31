using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System.Reflection;
using System.Xml;

namespace MySP2010Utilities
{
    class WebPartOperations : IWebPartOperations
    {
        public XsltListViewWebPart AddListToPage(SPList list, string title, string zone, SPLimitedWebPartManager webPartManager, int index)
        {
            // validation
            list.RequireNotNull("list");
            title.RequireNotNullOrEmpty("title");
            zone.RequireNotNullOrEmpty("zone");
            webPartManager.RequireNotNull("webPartManager");
            index.Require(index >= 0, "index");

            XsltListViewWebPart wp = new XsltListViewWebPart();
            wp.ListName = list.ID.ToString("B").ToUpper();
            wp.Title = title;
            wp.ZoneID = zone;
            ModifyViewClass viewOperations = new ModifyViewClass();
            SPView defaultView = viewOperations.GetDefaultView(list);
            SPView modifiedView = viewOperations.CopyView(defaultView, list);
            viewOperations.SetToolbarType(modifiedView, "Standard");
            modifiedView.Update();
            wp.ViewGuid = modifiedView.ID.ToString("B").ToUpper();
            webPartManager.AddWebPart(wp, zone, index);
            list.Update();
            webPartManager.SaveChanges(wp);
            return wp;
        }

        public void AddListToPage(Microsoft.SharePoint.SPList list, string title, string zone, Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager, int index, string viewName)
        {
            SharePointUtilities.AddListToPage(list, title, zone, webPartManager, index, viewName);
        }

        public void AddWebPart(Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager, System.Web.UI.WebControls.WebParts.WebPart webPart, string zone, int zoneIndex, System.Web.UI.WebControls.WebParts.PartChromeType chromeType, string accesskey)
        {
            SharePointUtilities.AddWebPart(webPartManager, webPart, zone, zoneIndex, chromeType, accesskey);
        }

        public void ConnectListViewWebParts(Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager, Microsoft.SharePoint.WebPartPages.ListViewWebPart providerWebPart, Microsoft.SharePoint.WebPartPages.ListViewWebPart consumerWebPart, Microsoft.SharePoint.WebPartPages.SPRowToParametersTransformer transformer, string consumerInternalFieldName, string providerInternalFieldName)
        {
            SharePointUtilities.ConnectListViewWebParts(webPartManager, providerWebPart, consumerWebPart, transformer, consumerInternalFieldName, providerInternalFieldName);
        }

        public void CreateContentEditorWebPart(Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager, string Content, string zone, int zoneIndex, System.Web.UI.WebControls.WebParts.PartChromeType chromeType, string webPartTitle)
        {
            SharePointUtilities.CreateContentEditorWebPart(webPartManager, Content, zone, zoneIndex, chromeType, webPartTitle);
        }
    }
}
