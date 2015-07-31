using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;

namespace MySP2010Utilities
{
    public interface IWebPartOperations
    {
        XsltListViewWebPart AddListToPage(SPList list, string title, string zone, SPLimitedWebPartManager webPartManager, int index);
        void AddListToPage(SPList list, string title, string zone, SPLimitedWebPartManager webPartManager, int index, string viewName);
        void AddWebPart(SPLimitedWebPartManager webPartManager, System.Web.UI.WebControls.WebParts.WebPart webPart, string zone, int zoneIndex, PartChromeType chromeType, string accesskey);
        void ConnectListViewWebParts(SPLimitedWebPartManager webPartManager, ListViewWebPart providerWebPart, ListViewWebPart consumerWebPart, SPRowToParametersTransformer transformer, string consumerInternalFieldName, string providerInternalFieldName);
        void CreateContentEditorWebPart(SPLimitedWebPartManager webPartManager, string Content, string zone, int zoneIndex, PartChromeType chromeType, string webPartTitle);
    }
}
