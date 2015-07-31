using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace MySP2010Utilities
{
    class ListTemplateFinder : MySP2010Utilities.IListTemplateFinder
    {
        public SPListTemplate GetListTemplate(SPListTemplateCollection listTemplates, string templateName)
        {
            listTemplates.RequireNotNull("listTemplates");
            templateName.RequireNotNullOrEmpty("templateName");

            try
            {
                return listTemplates[templateName];
            }
            catch (Exception exception)
            {
                LogUtility logUtility = new LogUtility();
                logUtility.TraceDebugException("Can't find list template", GetType(), exception);
                return null;
            }
        }
    }
}
