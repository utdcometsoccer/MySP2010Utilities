using System.Collections.Generic;
using Microsoft.SharePoint;
using System.Linq;
using System;
namespace MySP2010Utilities
{
    class ListOperations : IListOperations
    {

        public void AddField(SPList list, SPField field)
        {
            SharePointUtilities.AddFieldToList(list, field);
        }

        public void AddField(SPList list, IEnumerable<SPField> fields)
        {
            SharePointUtilities.AddFieldToList(list, fields);
        }

        public SPList CreateList(SPWeb webSite, string listName, string listDescription, SPListTemplateType listTemplate)
        {
            return SharePointUtilities.CreateList(webSite, listName, listDescription, listTemplate);
        }

        public void ChangeTitleDisplayName(SPList list, string newTitle)
        {
            SharePointUtilities.ChangeTitleDisplayName(list, newTitle);
        }

        public void ChangeTitleDisplayName(SPContentType contentType, string newTitle)
        {
            SharePointUtilities.ChangeTitleDisplayName(contentType, newTitle);
        }


        public SPList CreateWiki(SPWeb web, string title, string description)
        {
            return SharePointUtilities.CreateWiki(web, title, description);
        }

        public SPList CreateLibraryFromNamedTemplate(SPWeb web, string title, string description, string templateName)
        {
            return SharePointUtilities.CreateLibraryFromNamedTemplate(web, title, description, templateName);
        }

        public void DeleteList(SPWeb web, string ListTitle)
        {
            SharePointUtilities.DeleteList(web, ListTitle);
        }

        public SPFile CreatePage(SPList list, string fileName, SPTemplateFileType fileType)
        {
            return SharePointUtilities.CreatePage(list, fileName, fileType);
        }        

        public SPList CreateList(SPWeb web, string Title, string Description, SPListTemplate template)
        {
            web.RequireNotNull("web");
            Title.RequireNotNullOrEmpty("Title");
            Description.RequireNotNullOrEmpty("Description");
            template.RequireNotNull("template");
            SPList list = web.Lists.TryGetList(Title);

            return null != list ? list : createListImpl(web, Title, Description, template) ;
        }

        private static SPList createListImpl(SPWeb web, string Title, string Description, SPListTemplate template)
        {
            web.RequireNotNull("web");
            Title.RequireNotNullOrEmpty("Title");
            Description.RequireNotNullOrEmpty("Description");
            template.RequireNotNull("template");
            Guid listGuid = web.Lists.Add(Title, Description, template);
            return web.Lists[listGuid];
        }
    }
}
