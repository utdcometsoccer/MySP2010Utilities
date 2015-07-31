using System;
namespace MySP2010Utilities
{
    /// <summary>
    /// Interface for classes that register a custom document router
    /// </summary>
    public interface ICustomContentOrganizerRouterRegistrar
    {
        void RegisterCustomDocumentRouter(string absoluteSiteURL, string customRouterName, string customRouterAssemblyName, string customRouterClassName);
    }
}
