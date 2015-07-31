using System.Collections.Generic;
using Microsoft.SharePoint;

namespace MySP2010Utilities
{
    public interface IListOperations
    {
        void AddField(SPList list, SPField field);
        void AddField(SPList list, IEnumerable<SPField> fields);
        SPList CreateList(SPWeb webSite, string listName, string listDescription, SPListTemplateType listTemplate);
        SPList CreateWiki(SPWeb web, string title, string description);
        SPList CreateLibraryFromNamedTemplate(SPWeb web, string title, string description, string templateName);
        void ChangeTitleDisplayName(SPList list, string newTitle);
        void ChangeTitleDisplayName(SPContentType contentType, string newTitle);
        void DeleteList(SPWeb web, string ListTitle);
        SPFile CreatePage(SPList list, string fileName, SPTemplateFileType fileType);
        SPList CreateList(SPWeb web, string Title, string Description, SPListTemplate template);
    }
}
