using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
namespace MySP2010Utilities
{
    class ModifyContentType : IModifyContentType
    {

        public SPContentType CreateContentType(SPContentType parentType, SPWeb web, string contentTypeName, string contentTypeGroup, IEnumerable<SPField> fields)
        {
            return SharePointUtilities.CreateContentType(parentType, web, contentTypeName, contentTypeGroup, fields);
        }

        public void MakeDefaultContentType(SPList list, SPContentType contentType)
        {
            SharePointUtilities.MakeDefaultContentType(list, contentType);
        }


        public void DeleteContentType(SPWeb web, string contentTypeName)
        {
            SharePointUtilities.DeleteContentType(web, contentTypeName);
        }
    }
}
