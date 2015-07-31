using Microsoft.SharePoint;
using System;
namespace MySP2010Utilities
{
    public interface ILinkToDocumentCreator
    {
        SPFile AddDocumentLink(string webURL, string libraryName, string documentPath, string documentName, string documentUrl);
        SPFile AddDocumentLink(SPWeb web, string libraryName, string documentPath, string documentName, string documentUrl);
        SPFile AddDocumentLink(SPWeb web, SPList docLibrary, string documentPath, string documentName, string documentUrl);
        SPFile AddDocumentLink(SPWeb web, SPFolder targetFolder, string documentPath, string documentName, string documentUrl);
        SPFile AddDocumentLink(string url, string documentName, string documentUrl);
    }
}
