using System;
using Microsoft.SharePoint;
namespace MySP2010Utilities
{
    public interface IListTemplateFinder
    {
        SPListTemplate GetListTemplate(SPListTemplateCollection listTemplates, string templateName);
    }
}
