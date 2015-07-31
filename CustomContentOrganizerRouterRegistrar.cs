using Microsoft.Office.RecordsManagement.RecordsRepository;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySP2010Utilities
{
    /// <summary>
    /// Register a custom document router
    /// </summary>
    class CustomContentOrganizerRouterRegistrar : MySP2010Utilities.ICustomContentOrganizerRouterRegistrar
    {
        /// <summary>
        /// Registers the custom document router.
        /// </summary>
        /// <param name="absoluteSiteURL">The absolute site URL.</param>
        /// <param name="customRouterName">Name of the custom router.</param>
        /// <param name="customRouterAssemblyName">Name of the custom router assembly.</param>
        /// <param name="customRouterClassName">Name of the custom router class.</param>
        public void RegisterCustomDocumentRouter(string absoluteSiteURL, string customRouterName, string customRouterAssemblyName, string customRouterClassName)
        {
            absoluteSiteURL.RequireNotNullOrEmpty("absoluteSiteURL");
            customRouterAssemblyName.RequireNotNullOrEmpty("customRouterAssemblyName");
            customRouterClassName.RequireNotNullOrEmpty("customRouterClassName");
            customRouterName.RequireNotNullOrEmpty("customRouterName");

            using (SPSite site = new SPSite(absoluteSiteURL))
            using (SPWeb web = site.OpenWeb())
            {
                EcmDocumentRoutingWeb contentOrganizer = new EcmDocumentRoutingWeb(web);
                contentOrganizer.AddCustomRouter(customRouterName, customRouterAssemblyName, customRouterClassName);
            }
        }
    }
}
