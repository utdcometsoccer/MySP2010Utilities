using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using System.Reflection;
using System.Xml;

namespace MySP2010Utilities
{
    class ModifyViewClass : IViewOperations
    {
        public void ModifyView(SPList list, IEnumerable<string> ViewFields, string query)
        {
            SharePointUtilities.ModifyView(list, ViewFields, query);
        }

        public void ModifyView(SPView view, IEnumerable<string> ViewFields, string query)
        {
            SharePointUtilities.ModifyView(view, ViewFields, query);
        }


        public SPView GetDefaultView(SPList list)
        {
            return SharePointUtilities.GetDefaultView(list);
        }

        public void SetToolbarType(SPView spView, string toolBarType)
        {

            spView.GetType().InvokeMember("EnsureFullBlownXmlDocument",
             BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod,
             null, spView, null, System.Globalization.CultureInfo.CurrentCulture);
            PropertyInfo nodeProp = spView.GetType().GetProperty("Node",
            BindingFlags.NonPublic | BindingFlags.Instance);
            XmlNode node = nodeProp.GetValue(spView, null) as XmlNode;

            XmlNode toolbarNode = node.SelectSingleNode("Toolbar");
            if (toolbarNode != null)
            {
                toolbarNode.Attributes["Type"].Value = toolBarType;
            }
        }

        public SPView CopyView(SPView view, SPList list)
        {            
            System.Collections.Specialized.StringCollection viewFields = view.ViewFields.ToStringCollection();
            return list.Views.Add(view.Title+" Copy", viewFields, view.Query, view.RowLimit, view.Paged, false);
        }
    }
}
