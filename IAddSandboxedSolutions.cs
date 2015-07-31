using Microsoft.SharePoint;

namespace MySP2010Utilities
{
    interface IAddSandboxedSolutions
    {
        SPUserSolution AddSandboxedSolution(SPSite site, byte[] fileData, string solutionName);
        SPUserSolution AddSandboxedSolution(SPSite site, string path, string solutionName);
    }
}
