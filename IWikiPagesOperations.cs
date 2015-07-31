using System;
using Microsoft.SharePoint;
namespace MySP2010Utilities
{
    public interface IWikiPagesOperations
    {
        void ChangeWikiContent(Microsoft.SharePoint.SPFile wikiFile, string content);
        void EnsureSitePagesLibrary(Microsoft.SharePoint.SPWeb web, Microsoft.SharePoint.SPList sitePagesLibrary);
        Microsoft.SharePoint.SPList GetSitePagesLibrary(Microsoft.SharePoint.SPWeb web);
        void InsertWebPartIntoWikiPage(Microsoft.SharePoint.SPFile wikiFile, System.Web.UI.WebControls.WebParts.WebPart webpart, string replaceToken);
        string StorageKeyToID(Guid storageKey);
        void AddListToPage(SPFile homePage, SPList list);
        void ClearWikiPage(SPFile wikiFile, SPWeb web);
    }
}
