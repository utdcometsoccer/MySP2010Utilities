using Microsoft.SharePoint;
using System;


namespace MySP2010Utilities
{
    class FieldOperations : IFieldOperations
    {
        public SPField TryGetField(SPFieldCollection siteColumns, string fieldName)
        {
            return SharePointUtilities.TryGetField(siteColumns, fieldName);
        }

        public SPField TryGetField(SPFieldCollection siteColumns, Guid fieldID)
        {
            siteColumns.RequireNotNull("siteColumns");
            fieldID.Require(Guid.Empty != fieldID, "fieldID");

            return siteColumns.Contains(fieldID) ? siteColumns[fieldID] : null;
        }

        public SPFieldLookup CreateLookup(SPFieldCollection siteColumns, string fieldName, SPList lookupList, SPWeb web, bool required)
        {
            siteColumns.RequireNotNull("siteColumns");
            fieldName.RequireNotNullOrEmpty("fieldName");
            lookupList.RequireNotNull("lookupList");
            string internalFieldName;
            SPField looupField = TryGetField(siteColumns,fieldName);
            if (null == looupField)
            {
                if (null != web)
                {
                    internalFieldName = siteColumns.AddLookup(fieldName, lookupList.ID, web.ID, required);
                }
                else
                {
                    internalFieldName = siteColumns.AddLookup(fieldName, lookupList.ID, required);
                }
                looupField = siteColumns.GetFieldByInternalName(internalFieldName); 
            }
            return looupField as SPFieldLookup;
        }
    }
}
