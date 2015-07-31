using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace MySP2010Utilities
{
    class WorkflowMessageParser : MySP2010Utilities.IWorkflowMessageParser
    {
        public string parseMessage(string message, SPListItem item)
        {
            foreach (SPField field in item.ParentList.Fields)
            {
                string itemValue = null != item[field.Title] ? item[field.Title].ToString() : string.Empty;
                int hashIndex = itemValue.IndexOf('#');
                if (hashIndex != -1)
                {
                    itemValue = itemValue.Substring(hashIndex + 1);
                }
                message = message.Replace(string.Format("{{{0}}}", field.Title), itemValue);
            }

            return message;
        }
    }
}
