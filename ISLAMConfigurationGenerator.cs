using System;
namespace MySP2010Utilities
{
    public interface ISLAMConfigurationGenerator
    {
        byte[] WriteSlamConfig(Microsoft.SharePoint.Administration.SPWebApplication webApplication, string connectionString);
    }
}
