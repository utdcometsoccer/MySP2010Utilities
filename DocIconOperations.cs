using System.Xml.Linq;

namespace MySP2010Utilities
{
    class DocIconOperations :IDocIconOperations
    {
        public string RelativeDocIconPath
        {
            get { return SharePointUtilities.DocIconPath; }
        }

        public XElement DocIconXML
        {
            get { return SharePointUtilities.DocIconXML; }
        }

        public string GetIconPath(string fileExt)
        {
            return SharePointUtilities.GetIconPath(fileExt);
        }
    }
}
