using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System.Web.UI.WebControls.WebParts;
using System.Globalization;

namespace MySP2010Utilities
{
    class WikiPagesOperations : MySP2010Utilities.IWikiPagesOperations
    {
        public SPList GetSitePagesLibrary(SPWeb web)
        {
            SPList wikiList;
            try
            {
                string serverRelativeUrl = web.ServerRelativeUrl;
                if (serverRelativeUrl == "/")
                {
                    serverRelativeUrl = "/SitePages";
                }
                else
                {
                    serverRelativeUrl = serverRelativeUrl + "/SitePages";
                }

                wikiList = web.GetList(serverRelativeUrl);
            }
            catch
            {
                wikiList = null;
            }
            if ((wikiList != null) && (wikiList.BaseTemplate != SPListTemplateType.WebPageLibrary))
            {
                wikiList = null;
            }

            return wikiList;
        }

        public void ChangeWikiContent(SPFile wikiFile, string content)
        {
            wikiFile.RequireNotNull("wikiFile");
            wikiFile.Item["WikiField"] = content;
            wikiFile.Item.Update();
        }

        public void InsertWebPartIntoWikiPage(SPFile wikiFile, System.Web.UI.WebControls.WebParts.WebPart webpart, string replaceToken)
        {
            wikiFile.RequireNotNull("wikiFile");
            webpart.RequireNotNull("webpart");
            string str = (string)wikiFile.Item["WikiField"];


            using (SPLimitedWebPartManager limitedWebPartManager = wikiFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
            {
                Guid storageKey = Guid.NewGuid();
                string str2 = StorageKeyToID(storageKey);
                webpart.ID = str2;
                limitedWebPartManager.AddWebPart(webpart, "wpz", 0);
                string str3 = string.Format(CultureInfo.InvariantCulture, "<div class='ms-rtestate-read ms-rte-wpbox' contentEditable='false'><div class='ms-rtestate-read {0}' id='div_{0}'></div><div style='display:none' id='vid_{0}'/></div>",
                                            new object[] 
                                        { 
                                            storageKey.ToString("D") 
                                        });
                if (str == null)
                {
                    str = str3;
                }
                else
                {
                    if (!str.Contains(replaceToken))
                    {
                        str = str + str3;
                    }
                    else
                    {
                        str = str.Replace(replaceToken, str3);
                    }
                }
                wikiFile.Item["WikiField"] = str; wikiFile.Item.Update();
            }
        }

        public void ClearWikiPage(SPFile wikiFile, SPWeb web)
        {
            wikiFile.RequireNotNull("wikiFile");
            web.RequireNotNull("web");
            ChangeWikiContent(wikiFile, string.Empty);
            using (SPLimitedWebPartManager limitedWebPartManager = wikiFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
            {
                List<Microsoft.SharePoint.WebPartPages.WebPart> webParts =
                    new List<Microsoft.SharePoint.WebPartPages.WebPart>(
                        from Microsoft.SharePoint.WebPartPages.WebPart w in limitedWebPartManager.WebParts
                        select w);
                webParts.ForEach(w => limitedWebPartManager.DeleteWebPart(w));
            }
            web.Update();
        }

        public string StorageKeyToID(Guid storageKey)
        {
            if (!(Guid.Empty == storageKey))
            {
                return ("g_" + storageKey.ToString().Replace('-', '_'));
            }
            return string.Empty;
        }

        public void EnsureSitePagesLibrary(SPWeb web, SPList sitePagesLibrary)
        {
            if (sitePagesLibrary == null)
            {
                sitePagesLibrary = web.Lists.EnsureSitePagesLibrary();
            }

            if (sitePagesLibrary == null)
            {
                throw new SPException(SPResource.GetString("ListGone", new object[0]));
            }

            if (sitePagesLibrary.BaseTemplate != SPListTemplateType.WebPageLibrary)
            {
                throw new SPException(SPResource.GetString("OnlyInWikiLibraries", new object[0]));
            }

            if (sitePagesLibrary.ParentWeb != web)
            {
                throw new SPException(SPResource.GetString("WikiNotInWebException", new object[0]));
            }
        }

        public void AddListToPage(SPFile homePage, SPList list)
        {
            XsltListViewWebPart wp = new XsltListViewWebPart();
            wp.ListName = list.ID.ToString("B").ToUpper();
            ModifyViewClass viewOperations = new ModifyViewClass();
            SPView defaultView = viewOperations.GetDefaultView(list);
            SPView copiedView = viewOperations.CopyView(defaultView, list);
            viewOperations.SetToolbarType(copiedView, "Standard");
            wp.ViewGuid = defaultView.ID.ToString("B").ToUpper();
            wp.Title = list.Title;
            InsertWebPartIntoWikiPage(homePage, wp, "{{1}}");
        }
    }
}
