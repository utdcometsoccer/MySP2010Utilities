using System;
namespace MySP2010Utilities
{
    interface IAutofolderCreator
    {
        void CreateAutofolder(IContentOrganizerRuleCreationData data, Microsoft.SharePoint.SPContentType ruleContentType, Microsoft.Office.RecordsManagement.RecordsRepository.EcmDocumentRouterRule organizeDocument);
    }
}
