using System.Xml.Linq;

namespace MySP2010Utilities
{
    public interface IDocIconOperations
    {
        string RelativeDocIconPath { get; }
        XElement DocIconXML { get; }
        string GetIconPath(string fileExt);
    }
}
