using System.IO;
using Microsoft.SharePoint;

namespace MySP2010Utilities
{
     class SPFileOperations : ISPFileOperations
    {
        public void CopyStreams(Stream source, Stream destination)
        {
            SharePointUtilities.CopyStreams(source, destination);
        }

        public SPFile UploadStream(SPDocumentLibrary library, Stream stream, string fileName)
        {
            return SharePointUtilities.UploadStream(library, stream, fileName);
        }

        public SPFile UploadFromPath(SPDocumentLibrary library, string path, string fileName)
        {
            return SharePointUtilities.UploadFromPath(library, path, fileName);
        }


        public string LoadFromSharePointRoot(string relativePath)
        {
            return SharePointUtilities.LoadFromSharePointRoot(relativePath);
        }


    }
}
