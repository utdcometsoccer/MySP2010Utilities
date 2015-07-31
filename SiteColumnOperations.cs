using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;

namespace MySP2010Utilities
{
    class SiteColumnOperations : ISiteColumnOperations
    {
        public SPFieldChoice CreateChoiceSiteColumn(SPWeb web, string fieldName, IEnumerable<string> choices, bool required)
        {
            return SharePointUtilities.CreateChoiceSiteColumn(web, fieldName, choices, required);
        }

        public SPFieldLookup CreateLookupSiteColumn(SPWeb web, SPList list, string title, bool required)
        {
            return SharePointUtilities.CreateLookupSiteColumn(web, list, title, required);
        }

        public SPField CreateSiteColumn(SPWeb web, string fieldName, SPFieldType spFieldType, bool required)
        {
            return SharePointUtilities.CreateSiteColumn(web, fieldName, spFieldType, required);
        }

        public TaxonomyField CreateMangedMetadataSiteColumn(SPWeb web, string fieldName, TermSet termSet, string GroupName)
        {
            return SharePointUtilities.CreateMangedMetadataSiteColumn(web, fieldName, termSet, GroupName);
        }
    }
}
