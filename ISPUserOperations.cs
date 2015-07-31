using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MySP2010Utilities
{
    public interface ISPUserOperations
    {
        SPUser GetSPUser(SPListItem item, string key);
        string GetPickerEntities(PeopleEditor Editor, char separator);
    }
}
