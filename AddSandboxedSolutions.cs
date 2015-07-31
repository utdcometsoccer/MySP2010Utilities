using Microsoft.SharePoint;

namespace MySP2010Utilities
{
     class AddSandboxedSolutions : IAddSandboxedSolutions
    {
        public SPUserSolution AddSandboxedSolution(SPSite site, byte[] fileData, string solutionName)
        {
            return SharePointUtilities.AddSandboxedSolution(site,fileData,solutionName);
        }

        public SPUserSolution AddSandboxedSolution(SPSite site, string path, string solutionName)
        {
            return SharePointUtilities.AddSandboxedSolution(site,path,solutionName);
        }
    }
}
