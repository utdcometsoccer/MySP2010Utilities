using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;

namespace MySP2010Utilities
{
    public interface ISiteColumnOperations
    {
        SPFieldChoice CreateChoiceSiteColumn(SPWeb web, string fieldName, IEnumerable<string> choices, bool required);
        SPFieldLookup CreateLookupSiteColumn(SPWeb web, SPList list, string title, bool required);
        SPField CreateSiteColumn(SPWeb web, string fieldName, SPFieldType spFieldType, bool required);
        TaxonomyField CreateMangedMetadataSiteColumn(SPWeb web, string fieldName, TermSet termSet, string GroupName);
    }
}
