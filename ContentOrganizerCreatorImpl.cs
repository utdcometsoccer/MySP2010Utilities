using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.RecordsManagement.RecordsRepository;
using Microsoft.SharePoint;

namespace MySP2010Utilities
{
    class ContentOrganizerCreatorImpl : MySP2010Utilities.IContentOrganizerCreatorImpl
    {
        public void CreateRuleManagedMetadataField(IContentOrganizerRuleCreationData data, string conditionsXml, SPSite site, SPWeb web, IAutofolderCreator creator)
        {            
            EcmDocumentRoutingWeb contentOrganizerSiteWrapper = new EcmDocumentRoutingWeb(web);
            IContentTypeOperations contentTypeOps = new ContentTypeOperations();
            SPContentType ruleContentType = contentTypeOps.TryFindContentType(site.RootWeb, data.ContentTypeName);
            SPList ruleLibrary = null;
            if (null != ruleContentType)
            {
                ruleLibrary = web.Lists[data.LibraryName];
                if (ruleLibrary.ContentTypes.BestMatch(ruleContentType.Id) == null)
                {
                    throw new ArgumentException(String.Format(
                        "Ensure that the library {0} contains content type {1} before creating the rule",
                        data.LibraryName,
                        data.ContentTypeName));
                }
            }

            else
            {
                throw new Exception("Content Type does not exist!");
            }

            EcmDocumentRouterRule organizeDocument = new EcmDocumentRouterRule(web);
            // Configure the rule to specify conditions that match the rule and final location for documents matching this rule.
            organizeDocument.Name = data.Name;
            organizeDocument.Description = data.Description;
            // Configure the rule so that it will be evaluated on documents of "Contract" content type
            organizeDocument.ContentTypeString = ruleContentType.Name;
            organizeDocument.RouteToExternalLocation = false;
            // Set a priority for this rule which indicates the order in which rules are executed. This is a number between 0 and 9.
            organizeDocument.Priority = data.Priority;
            // Specify where the documents that match this rule get saved to.
            // To route documents externally, the TargetPath value can be set to one of the SendTo connections configured for this web application or site subscription.
            // Example: organizeDocument.TargetPath = contentOrganizerSiteCollection.WebApplication.OfficialFileHosts[0];
            organizeDocument.TargetPath = ruleLibrary.RootFolder.ServerRelativeUrl;

            // Set the conditions string for this rule
            organizeDocument.ConditionsString = conditionsXml;
            // AutoFolder configuration: Optionally enable automatic folder creation for this rule based on a non-empty (required or boolean) field. 
            // Folders will be created for each unique value of this field in the TargethPath and documents will be saved here.

            if (!string.IsNullOrEmpty(data.AutoFolderPropertyName))
            {
                creator.CreateAutofolder(data, ruleContentType, organizeDocument);
            }
            if (!string.IsNullOrEmpty(data.CustomRouter))
            {
                organizeDocument.CustomRouter = data.CustomRouter;
            }
            // Update the rule and commit changes.
            organizeDocument.Update();
        }
    }
}
