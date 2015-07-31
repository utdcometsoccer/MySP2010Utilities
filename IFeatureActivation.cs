using System;
using Microsoft.SharePoint;

namespace MySP2010Utilities
{
    public interface IFeatureActivator
    {
        SPFeature ActivateFeatureIfNecessary(SPSite site, Guid featureGuid);
        SPFeature ActivateFeatureIfNecessary(SPWeb web, Guid featureGuid);
        SPFeature ActivateFeatureIfNecessary(SPWeb web, Guid featureGuid, bool force, SPFeatureDefinitionScope sPFeatureDefinitionScope);
        SPFeature ActivateFeatureIfNecessary(SPSite site, Guid featureGuid, bool force, SPFeatureDefinitionScope sPFeatureDefinitionScope);
    }
}
