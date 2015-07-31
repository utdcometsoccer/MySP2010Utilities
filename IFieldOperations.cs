using Microsoft.SharePoint;
using System;

namespace MySP2010Utilities
{
    public interface IFieldOperations
    {
        SPField TryGetField(SPFieldCollection siteColumns, string fieldName);
        SPField TryGetField(SPFieldCollection siteColumns, Guid fieldID);
        SPFieldLookup CreateLookup(SPFieldCollection siteColumns, string fieldName, SPList lookupList, SPWeb web, bool required);
    }
}
