using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace MySP2010Utilities
{
    public interface IWorkflowOperations
    {        
        void AddWorkflow(SPWeb web, SPList list, SPList tasks, SPList workflowHistory, string workflowTemplateName, string workflowName);
        void AddWorkflow(SPWeb web, SPContentType contentType, SPList tasks, SPList workflowHistory, string workflowTemplateName, string workflowName);
        void AddWorkflow(SPWeb web, SPContentType contentType, SPList tasks, SPList workflowHistory, string workflowTemplateName, string workflowName, object associationData);
        SPWorkflowTemplate GetWorkflowByName(SPWeb web, string workflowName);
        void EnableContentApproval(SPList list);
        void DefaultContentApproval(SPWeb web, SPList list, SPList tasks, SPList workflowHistory, string workflowName);
        string GetDefaultAssociationData(SPWeb web, XDocument associationData);
        void RemoveSiteContentTypeAssociation(SPSite site, string associationName);
    }
}
