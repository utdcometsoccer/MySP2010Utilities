using System.Collections.Generic;
using Microsoft.SharePoint;

namespace MySP2010Utilities
{
    public interface IContentTypeOperations
    {
        void AddField(SPContentType cType, SPField field);
        void AddField(SPContentType cType, IEnumerable<SPField> fields);
        SPContentType CreateContentType(SPContentType parentType, SPWeb web, string contentTypeName, string contentTypeGroup, IEnumerable<SPField> fields);
        SPContentType TryFindContentType(SPWeb rootWeb, string contentTypeName);
        void ReorderContentTypeFields(SPContentType contentType, IEnumerable<string> fields);
        void AddEventReceiverToContentType(string className, SPContentType contentType, string assemblyName, SPEventReceiverType eventReceiverType, SPEventReceiverSynchronization eventReceiverSynchronization);
        bool containsField(SPContentType contentType, string DisplayName);
        void AddContentTypeToList(SPContentType contentType, SPList list);
    }
}
