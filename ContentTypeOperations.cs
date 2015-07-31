using System.Collections.Generic;
using Microsoft.SharePoint;
using System.Linq;
namespace MySP2010Utilities
{
    class ContentTypeOperations : IContentTypeOperations
    {
        public void AddField(SPContentType cType, SPField field)
        {
            SharePointUtilities.AddFieldToContentType(cType, field);
        }

        public void AddField(SPContentType cType, IEnumerable<SPField> fields)
        {
            SharePointUtilities.AddFieldToContentType(cType, fields);
        }

        public SPContentType CreateContentType(SPContentType parentType, SPWeb web, string contentTypeName, string contentTypeGroup, IEnumerable<SPField> fields)
        {
            return SharePointUtilities.CreateContentType(parentType, web, contentTypeName, contentTypeGroup, fields);
        }

        public SPContentType TryFindContentType(SPWeb rootWeb, string contentTypeName)
        {
            return SharePointUtilities.TryFindContentType(rootWeb, contentTypeName);
        }

        public void ReorderContentTypeFields(SPContentType contentType, IEnumerable<string> fields)
        {
            SharePointUtilities.ReorderContentTypeFields(contentType, fields);
        }

        public void AddEventReceiverToContentType(string className, SPContentType contentType, string assemblyName, SPEventReceiverType eventReceiverType, SPEventReceiverSynchronization eventReceiverSynchronization)
        {
            SharePointUtilities.AddEventReceiverToContentType(className, contentType, assemblyName, eventReceiverType, eventReceiverSynchronization);
        }

        public bool containsField(SPContentType contentType, string DisplayName)
        {
            contentType.RequireNotNull("contentType");
            DisplayName.RequireNotNullOrEmpty("DisplayName");
            var fieldLink = from SPFieldLink fl in contentType.FieldLinks
                            where fl.DisplayName.Equals(DisplayName)
                            select fl;

            return contentType.Fields.ContainsField(DisplayName) || fieldLink.Count() > 0;
        }

        public void AddContentTypeToList(SPContentType contentType, SPList list)
        {
            contentType.RequireNotNull("contentType");
            list.RequireNotNull("list");
            list.ContentTypes.Add(contentType);
            list.Update();
        }
    }
}
