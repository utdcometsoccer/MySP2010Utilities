using System;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace MySP2010Utilities
{
    class WorkflowOperations : IWorkflowOperations
    {
        public void AddWorkflow(SPWeb web, SPList list, SPList tasks, SPList workflowHistory, string workflowTemplateName, string workflowName)
        {
            SharePointUtilities.AddWorkflow(web, list, tasks, workflowHistory, workflowTemplateName, workflowName);
        }

        public void AddWorkflow(SPWeb web, SPContentType contentType, SPList tasks, SPList workflowHistory, string workflowTemplateName, string workflowName)
        {
            SharePointUtilities.AddWorkflow(web, contentType, tasks, workflowHistory, workflowTemplateName, workflowName);
        }


        public void AddWorkflow(SPWeb web, SPContentType contentType, SPList tasks, SPList workflowHistory, string workflowTemplateName, string workflowName, object associationData)
        {
            SharePointUtilities.AddWorkflow(web, contentType, tasks, workflowHistory, workflowTemplateName, workflowName, associationData);
        }

        public Microsoft.SharePoint.Workflow.SPWorkflowTemplate GetWorkflowByName(SPWeb web, string workflowName)
        {
            return SharePointUtilities.GetWorkflowByName(web, workflowName);
        }

        public void EnableContentApproval(SPList list)
        {
            SharePointUtilities.EnableContentApproval(list);
        }


        public void DefaultContentApproval(SPWeb web, SPList list, SPList tasks, SPList workflowHistory, string workflowName)
        {
            SharePointUtilities.DefaultContentApproval(web, list, tasks, workflowHistory, workflowName);
        }

        public string GetDefaultAssociationData(SPWeb web, XDocument associationData)
        {
            return SharePointUtilities.GetDefaultAssociationData(web, associationData);
        }

        public void RemoveSiteContentTypeAssociation(SPSite site, string associationName)
        {
            LogUtility logUtility = new LogUtility();
            SPWeb rootWeb = site.RootWeb;
            SPWorkflowAssociation association = null;
            logUtility.TraceDebugInformation(string.Format("Removing site contenttype workflow association '{0}'.", associationName), GetType());
            // Remove all site content type workflow associations
            foreach (SPContentType ctype in rootWeb.ContentTypes)
            {
                // Remove site content type workflow association
                association = ctype.WorkflowAssociations.GetAssociationByName(associationName, rootWeb.Locale);
                if (association != null)
                {
                    ctype.WorkflowAssociations.Remove(association);
                }
            }

            // Remove list content type workflow associations
            foreach (SPWeb web in site.AllWebs)
            {
                // Since we plan to modify some lists later we can not use the standard web.Lists collection
                System.Collections.Generic.List<Guid> lists = new System.Collections.Generic.List<Guid>();
                foreach (SPList list in web.Lists) lists.Add(list.ID);

                // Check all lists
                foreach (Guid listId in lists)
                {
                    // Get list
                    SPList list = web.Lists[listId];

                    // Remove all matching list content type associations
                    foreach (SPContentType ctype in list.ContentTypes)
                    {
                        // Remove list content type workflow association
                        association = ctype.WorkflowAssociations.GetAssociationByName(associationName, web.Locale);
                        if (association != null)
                        {
                            ctype.WorkflowAssociations.Remove(association);
                        }
                    }

                    // Find all workflow status fields in the list
                    System.Collections.Generic.List<Guid> fields = new System.Collections.Generic.List<Guid>();
                    foreach (SPField field in list.Fields)
                    {
                        // Check field and save Guid on match
                        if (field.Type == SPFieldType.WorkflowStatus && field.Title == associationName)
                        {
                            fields.Add(field.Id);
                        }
                    }

                    // Remove the fields from the list
                    foreach (Guid fieldId in fields)
                    {
                        SPField f = list.Fields[fieldId];   // Get the status field
                        f.ReadOnlyField = false;            // Flag read/write
                        f.Update(true);                     // Update first otherwise the field cannot be deleted
                        f.Delete();                         // Now delete
                    }

                    // Save modified list in database
                    list.Update();
                }
                web.Dispose();

            }
        }
    }
}
