using System;
namespace MySP2010Utilities
{
    interface IContentOrganizerCreatorImpl
    {
        void CreateRuleManagedMetadataField(IContentOrganizerRuleCreationData data, string conditionsXml, Microsoft.SharePoint.SPSite site, Microsoft.SharePoint.SPWeb web, IAutofolderCreator creator);
    }
}
