using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace MySP2010Utilities
{
    public interface IViewOperations
    {
        void ModifyView(SPList list, IEnumerable<string> ViewFields, string query);
        void ModifyView(SPView view, IEnumerable<string> ViewFields, string query);
        SPView GetDefaultView(SPList list);
        void SetToolbarType(SPView spView, string toolBarType);
        SPView CopyView(SPView view, SPList list);
    }
}
