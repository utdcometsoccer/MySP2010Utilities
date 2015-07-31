using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace MySP2010Utilities
{
    public interface IModifyContentType
    {
        SPContentType CreateContentType(SPContentType parentType, SPWeb web, string contentTypeName, string contentTypeGroup, IEnumerable<SPField> fields);
        void MakeDefaultContentType(SPList list, SPContentType contentType);
        void DeleteContentType(SPWeb web, string contentTypeName);
    }
}
