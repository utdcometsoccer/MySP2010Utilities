using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MySP2010Utilities
{
    class SPUserOperations : ISPUserOperations
    {
        public SPUser GetSPUser(SPListItem item, string key)
        {
            return SharePointUtilities.GetSPUser(item, key);
        }

        public string GetPickerEntities(PeopleEditor Editor, char separator)
        {
            return SharePointUtilities.GetPickerEntities(Editor, separator);
        }
    }
}
