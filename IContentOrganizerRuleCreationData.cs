using System;
using System.Collections.Generic;
namespace MySP2010Utilities
{
    public interface IContentOrganizerRuleCreationData
    {
        string AutoFolderNameFormat { get; set; }
        string AutoFolderPropertyName { get; set; }
        IList<IContentOrganizerConditionalData> Conditions { get; }
        string ContentTypeName { get; set; }
        string Description { get; set; }
        string LibraryName { get; set; }
        string Name { get; set; }
        string SiteAbsoluteUrl { get; set; }
        string Priority { get; set; }
        string CustomRouter { get; set; }
    }
}
