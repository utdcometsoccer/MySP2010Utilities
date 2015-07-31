using System;
using Microsoft.SharePoint;

namespace MySP2010Utilities
{
     class FeatureActivator : IFeatureActivator
    {
        public SPFeature ActivateFeatureIfNecessary(SPSite site, Guid featureGuid)
        {
            return SharePointUtilities.ActivateFeatureIfNecessary(site, featureGuid);
        }

        public SPFeature ActivateFeatureIfNecessary(SPWeb web, Guid featureGuid)
        {
            return SharePointUtilities.ActivateFeatureIfNecessary(web, featureGuid);
        }

        public SPFeature ActivateFeatureIfNecessary(SPWeb web, Guid featureGuid, bool force, SPFeatureDefinitionScope sPFeatureDefinitionScope)
        {
            return SharePointUtilities.ActivateFeatureIfNecessary(web, featureGuid, force, sPFeatureDefinitionScope);
        }

        public SPFeature ActivateFeatureIfNecessary(SPSite site, Guid featureGuid, bool force, SPFeatureDefinitionScope sPFeatureDefinitionScope)
        {
            return SharePointUtilities.ActivateFeatureIfNecessary(site, featureGuid, force, sPFeatureDefinitionScope);
        }
    }
}
