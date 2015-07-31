using System.IO;
using Microsoft.SharePoint;

namespace MySP2010Utilities
{
    public interface ISPFileOperations
    {
        void CopyStreams(Stream source, Stream destination);
        SPFile UploadStream(SPDocumentLibrary library, Stream stream, string fileName);
        SPFile UploadFromPath(SPDocumentLibrary library, string path, string fileName);
        string LoadFromSharePointRoot(string relativePath);
    }
}
