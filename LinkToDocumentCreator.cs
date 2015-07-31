using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MySP2010Utilities
{
    public class LinkToDocumentCreator : MySP2010Utilities.ILinkToDocumentCreator
    {
        const string aspxPageFormat = @"<%@ Assembly Name='{0}' %>
            <%@ Register TagPrefix='SharePoint' Namespace='Microsoft.SharePoint.WebControls' Assembly='Microsoft.SharePoint' %>
            <%@ Import Namespace='System.IO' %>
            <%@ Import Namespace='Microsoft.SharePoint' %>
            <%@ Import Namespace='Microsoft.SharePoint.Utilities' %>
            <%@ Import Namespace='Microsoft.SharePoint.WebControls' %>
                <html>
                    <head> 
                            <meta name='progid' content='SharePoint.Link' /> 
                    </head>
                    <body>
                        <form id='Form1' runat='server'>
                            <SharePoint:UrlRedirector id='Redirector1' runat='server' />
                        </form>
                    </body>
                </html>";

        public SPFile AddDocumentLink(string webURL, string libraryName, string documentPath, string documentName, string documentUrl)
        {
            using (SPSite site = new SPSite(webURL))
            using (SPWeb web = site.OpenWeb())
            {
                return AddDocumentLink(web, libraryName, documentPath, documentName, documentUrl);
            }
        }
        public SPFile AddDocumentLink(SPWeb web, string libraryName, string documentPath, string documentName, string documentUrl)
        {
            var docLibrary = web.Lists[libraryName];

            return AddDocumentLink(web, docLibrary, documentPath, documentName, documentUrl);
        }

        public SPFile AddDocumentLink(SPWeb web, SPList docLibrary, string documentPath, string documentName, string documentUrl)
        {
            return AddDocumentLink(web, docLibrary.RootFolder, documentPath, documentName, documentUrl);
        }

        public SPFile AddDocumentLink(SPWeb web, SPFolder targetFolder, string documentPath, string documentName, string documentUrl)
        {
            web.RequireNotNull("web");
            targetFolder.RequireNotNull("targetFolder");
            documentPath.RequireNotNullOrEmpty("documentPath");
            documentName.RequireNotNullOrEmpty("documentName");
            documentUrl.RequireNotNullOrEmpty("documentUrl");
            IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
            IContentTypeOperations contentTypeOps = serviceLocator.GetInstance<IContentTypeOperations>();
            string contentTypeName = "Link to a Document";
           
            var contentType = web.AvailableContentTypes[contentTypeName];
            SPDocumentLibrary DocLibrary = targetFolder.DocumentLibrary;
            if (null != DocLibrary)
            {
                bool LinkToDocumentApplied = false;
                foreach (SPContentType cType in DocLibrary.ContentTypes)
                {
                    if (cType.Name == contentTypeName)
                    {
                        LinkToDocumentApplied = true;
                        break;
                    }
                }

                if (!LinkToDocumentApplied)
                {
                    contentTypeOps.AddContentTypeToList(contentType, DocLibrary);
                }
            }

            var filePath = targetFolder.ServerRelativeUrl;
            if (!string.IsNullOrEmpty(documentPath))
            {
                filePath += "/" + documentPath;
            }
            var currentFolder = web.GetFolder(filePath);

            var files = currentFolder.Files;
            var urlOfFile = currentFolder.Url + "/" + documentName + ".aspx";


            var builder = new StringBuilder(aspxPageFormat.Length + 400);
            builder.AppendFormat(aspxPageFormat, typeof(SPDocumentLibrary).Assembly.FullName);

            var properties = new Hashtable();
            properties["ContentTypeId"] = contentType.Id.ToString();

            var file = files.Add(urlOfFile, new MemoryStream(new UTF8Encoding().GetBytes(builder.ToString())), properties, false, false);
            var item = file.Item;
            item["URL"] = documentUrl + ", ";
            item.UpdateOverwriteVersion();
            return file;
        }

        public SPFile AddDocumentLink(string url, string documentName, string documentUrl)
        {
            url.RequireNotNullOrEmpty("targetPath");

            using (SPSite site = new SPSite(url))
            using (SPWeb web = site.OpenWeb())
            {
                int index = url.LastIndexOf('/');
                string documentPath = url.Substring(index+1);
                string folderUrl = url.Substring(0, index);
                SPFolder folder = web.GetFolder(folderUrl);   
                return AddDocumentLink(web, folder, documentPath, documentName, documentUrl);
            }
        }
    }
}
