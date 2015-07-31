using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.Taxonomy;
using System.Diagnostics;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;
using System.Text;
using System.Xml.Serialization;

namespace MySP2010Utilities
{
    public static class SharePointUtilities
    {
        public static void SetMasterPage(SPSite siteCollection, string masterPagePath, string cssPath, string siteLogoPath, int uiVersion)
        {
            // validation
            if (null == siteCollection)
            {
                throw new ArgumentNullException("siteCollection");
            }

            if (string.IsNullOrEmpty(masterPagePath))
            {
                throw new ArgumentException("masterPagePath is null or empty");
            }

            if (string.IsNullOrEmpty(cssPath))
            {
                throw new ArgumentException("cssPath is null or empty");
            }

            if (string.IsNullOrEmpty(siteLogoPath))
            {
                throw new ArgumentException("siteLogoPath is null or empty");
            }

            if (uiVersion != 3 && uiVersion != 4)
            {
                throw new ArgumentException("uiVersion must be 3 or 4!");
            }


            SPWeb topLevelSite = siteCollection.RootWeb;

            // calculate relative path of site from Web Application root
            string WebAppRelativePath = GetWebAppRelativePath(topLevelSite);

            foreach (SPWeb site in siteCollection.AllWebs)
            {

                site.MasterUrl = string.Format("{0}{1}", WebAppRelativePath, masterPagePath);
                site.CustomMasterUrl = string.Format("{0}{1}", WebAppRelativePath, masterPagePath);
                site.AlternateCssUrl = string.Format("{0}{1}", WebAppRelativePath, cssPath);
                site.SiteLogoUrl = string.Format("{0}{1}", WebAppRelativePath, siteLogoPath);
                site.UIVersion = uiVersion;
                //site.UIVersionConfigurationEnabled = false;
                site.Update();
            }
        }

        public static string GetWebAppRelativePath(SPWeb topLevelSite)
        {
            string WebAppRelativePath = topLevelSite.ServerRelativeUrl;
            if (!WebAppRelativePath.EndsWith(@"/"))
            {
                WebAppRelativePath += @"/";
            }
            return WebAppRelativePath;
        }

        public static void SetMasterPage(SPSite siteCollection, string masterPagePath, int uiVersion)
        {
            // validation
            if (null == siteCollection)
            {
                throw new ArgumentNullException("siteCollection");
            }

            if (string.IsNullOrEmpty(masterPagePath))
            {
                throw new ArgumentException("masterPagePath is null or empty");
            }

            if (uiVersion != 3 && uiVersion != 4)
            {
                throw new ArgumentException("uiVersion must be 3 or 4!");
            }

            SPWeb topLevelSite = siteCollection.RootWeb;

            // calculate relative path of site from Web Application root
            string WebAppRelativePath = GetWebAppRelativePath(topLevelSite);
            foreach (SPWeb site in siteCollection.AllWebs)
            {

                site.MasterUrl = string.Format("{0}{1}", WebAppRelativePath, masterPagePath);
                site.CustomMasterUrl = string.Format("{0}{1}", WebAppRelativePath, masterPagePath);
                site.UIVersion = uiVersion;
                //site.UIVersionConfigurationEnabled = false;
                site.Update();
            }
        }

        public static void SetDefaultBranding(SPSite siteCollection)
        {
            // validation
            if (null == siteCollection)
            {
                throw new ArgumentNullException("siteCollection");
            }

            SPWeb topLevelSite = siteCollection.RootWeb;
            // calculate relative path of site from Web Application root
            string WebAppRelativePath = GetWebAppRelativePath(topLevelSite);
            // Enumerate through each site and remove custom branding.
            foreach (SPWeb site in siteCollection.AllWebs)
            {
                site.MasterUrl = WebAppRelativePath +
                                 "_catalogs/masterpage/v4.master";
                site.CustomMasterUrl = WebAppRelativePath +
                                       "_catalogs/masterpage/v4.master";
                site.AlternateCssUrl = "";
                site.SiteLogoUrl = "";
                site.Update();
            }

        }

        public static void PropagateParentBranding(SPWeb childSite)
        {
            // validation
            if (null == childSite)
            {
                throw new ArgumentNullException("childSite");
            }

            SPWeb topSite = childSite.Site.RootWeb;
            childSite.MasterUrl = topSite.MasterUrl;
            childSite.CustomMasterUrl = topSite.CustomMasterUrl;
            childSite.AlternateCssUrl = topSite.AlternateCssUrl;
            childSite.SiteLogoUrl = topSite.SiteLogoUrl;
            childSite.Update();
        }

        public static void SetTheme(SPSite siteCollection, string themeName)
        {
            // validation
            if (null == siteCollection)
            {
                throw new ArgumentNullException("siteCollection");
            }
            if (string.IsNullOrEmpty(themeName))
            {
                throw new ArgumentException("themeName is null or empty!");
            }
            using (SPWeb objWeb = siteCollection.OpenWeb())
            {

                ReadOnlyCollection<ThmxTheme>

                objThmxThemeList;

                objThmxThemeList =

                ThmxTheme.GetManagedThemes(siteCollection);

                foreach (ThmxTheme objThmxTheme in objThmxThemeList)
                {

                    if (objThmxTheme.Name == themeName)
                    {

                        ThmxTheme.SetThemeUrlForWeb(objWeb, objThmxTheme.ServerRelativeUrl);

                        break;

                    }

                }
            }
        }

        public static void SetDefaultTheme(SPSite siteCollection, string themeName)
        {
            // validation
            if (null == siteCollection)
            {
                throw new ArgumentNullException("siteCollection");
            }
            if (string.IsNullOrEmpty(themeName))
            {
                throw new ArgumentException("themeName is null or empty!");
            }


            using (SPWeb objWeb = siteCollection.OpenWeb())
            {

                ReadOnlyCollection<ThmxTheme>

                objThmxThemeList;

                objThmxThemeList =

                ThmxTheme.GetManagedThemes(siteCollection);

                foreach (ThmxTheme objThmxTheme in objThmxThemeList)
                {

                    if (objThmxTheme.Name == themeName)
                    {
                        ThmxTheme.SetThemeUrlForWeb(objWeb, null);
                        break;
                    }
                }
            }
        }

        public static SPList CreateList(SPWeb webSite, string listName, string listDescription, SPListTemplateType listTemplate)
        {
            // validation
            if (null == webSite)
            {
                throw new ArgumentNullException("webSite");
            }

            if (string.IsNullOrEmpty(listName))
            {
                throw new ArgumentException("listName is null or empty!");
            }

            if (string.IsNullOrEmpty(listDescription))
            {
                throw new ArgumentException("listDescription is null or empty!");
            }
            SPList list = webSite.Lists.TryGetList(listName);

            if (null == list)
            {
                Guid newListGuid = webSite.Lists.Add(listName, listDescription, listTemplate);
                list = webSite.Lists[newListGuid];
            }
            return list;
        }

        public static ListViewWebPart AddListToPage(SPList list, string title, string zone, SPLimitedWebPartManager webPartManager, int index)
        {
            // validation
            if (null == list)
            {
                throw new ArgumentNullException("list");
            }

            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("title is null or empty!");
            }

            if (null == webPartManager)
            {
                throw new ArgumentNullException("webPartManager");
            }

            ListViewWebPart wp = new ListViewWebPart();
            wp.ListName = list.ID.ToString("B").ToUpper();
            wp.Title = title;
            wp.ZoneID = zone;
            webPartManager.AddWebPart(wp, zone, index);
            list.Update();
            webPartManager.SaveChanges(wp);
            return wp;
        }

        public static void AddListToPage(SPList list, string title, string zone, SPLimitedWebPartManager webPartManager, int index, string viewName)
        {
            // validation
            list.RequireNotNull("list");
            title.RequireNotNullOrEmpty("title");
            webPartManager.RequireNotNull("webPartManager");
            viewName.RequireNotNullOrEmpty("viewName");

            ListViewWebPart wp = new ListViewWebPart();
            wp.ListName = list.ID.ToString("B").ToUpper();
            wp.ViewGuid = list.Views[viewName].ID.ToString("B").ToUpper();
            wp.Title = title;
            wp.ZoneID = zone;
            webPartManager.AddWebPart(wp, zone, index);
            list.Update();
            webPartManager.SaveChanges(wp);
        }

        public static void CreateContentEditorWebPart(SPLimitedWebPartManager webPartManager, string Content, string zone, int zoneIndex, PartChromeType chromeType, string webPartTitle)
        {
            // validation
            webPartManager.RequireNotNull("webPartManager");
            Content.RequireNotNullOrEmpty("Content");
            zone.RequireNotNullOrEmpty("zone");
            webPartTitle.RequireNotNullOrEmpty("webPartTitle");

            Guid storageKey = Guid.NewGuid();
            string wpId = String.Format("g_{0}", storageKey.ToString().Replace('-', '_'));
            XmlDocument doc = new XmlDocument();
            XmlElement div = doc.CreateElement("div");
            div.InnerText = Content;
            ContentEditorWebPart cewp = new ContentEditorWebPart { Content = div, ID = wpId, Title = webPartTitle };
            cewp.ChromeType = chromeType;
            webPartManager.AddWebPart(cewp, zone, zoneIndex);
            webPartManager.SaveChanges(cewp);
        }

        public static SPField CreateSiteColumn(SPWeb web, string fieldName, SPFieldType spFieldType, bool required)
        {
            // Validation
            web.RequireNotNull("web");
            fieldName.RequireNotNullOrEmpty("fieldName");

            if (web.AvailableFields.ContainsField(fieldName))
            {
                return web.AvailableFields[fieldName];
            }

            string internalName = web.Fields.Add(fieldName, spFieldType, required);
            return web.Fields.GetFieldByInternalName(internalName);

        }

        public static SPFieldLookup CreateLookupSiteColumn(SPWeb web, SPList list, string title, bool required)
        {
            // Validation
            web.RequireNotNull("web");
            list.RequireNotNull("List");
            title.RequireNotNullOrEmpty("Title");

            if (web.AvailableFields.ContainsField(title))
            {
                return web.AvailableFields[title] as SPFieldLookup;
            }

            string internalName = web.Fields.AddLookup(title, list.ID, required);
            SPFieldLookup lookupField = web.Fields.GetFieldByInternalName(internalName) as SPFieldLookup;
            return lookupField;
        }

        public static SPContentType CreateContentType(SPContentType parentType, SPWeb web, string contentTypeName, string contentTypeGroup, IEnumerable<SPField> fields)
        {
            // Validation
            parentType.RequireNotNull("parentType");
            web.RequireNotNull("spContentTypeCollection");
            contentTypeName.RequireNotNullOrEmpty("contentTypeName");
            contentTypeGroup.RequireNotNullOrEmpty("contentTypeGroup");
            fields.RequireNotNull("fields");

            SPContentType contentType = web.AvailableContentTypes[contentTypeName];
            if (null != contentType)
            {
                return contentType;
            }

            contentType = new SPContentType(parentType, web.ContentTypes, contentTypeName);
            contentType = web.ContentTypes.Add(contentType);
            contentType.Group = contentTypeGroup;

            foreach (SPField field in fields)
            {
                SPFieldLink fieldLink = new SPFieldLink(field);
                contentType.FieldLinks.Add(fieldLink);
            }
            contentType.Update();

            return contentType;
        }

        public static void MakeDefaultContentType(SPList list, SPContentType contentType)
        {
            //Validation
            list.RequireNotNull("list");
            contentType.RequireNotNull("contentType");

            SPContentType existingContentType = list.ContentTypes[contentType.Name];
            if (null == existingContentType)
            {
                list.ContentTypes.Add(contentType);
                list.Update();
            }

            List<SPContentType> cts = new List<SPContentType>();

            foreach (SPContentType item in list.ContentTypes)
            {
                if (item.Name.Equals(contentType.Name))
                {
                    cts.Add(item);
                }
            }
            list.RootFolder.UniqueContentTypeOrder = cts;
            list.RootFolder.Update();
        }

        public static void ModifyView(SPList list, IEnumerable<string> ViewFields, string query)
        {
            // Validation
            list.RequireNotNull("list");

            ViewFields.RequireNotNull("ViewFields");

            if (ViewFields.Count() == 0)
            {
                throw new ArgumentException("ViewFields");
            }

            query.RequireNotNullOrEmpty("query");

            SPView listView = (from SPView v in list.Views
                               where v.DefaultView == true
                               select v).First();
            listView.Query = query;

            listView.ViewFields.DeleteAll();
            foreach (string fieldName in ViewFields)
            {
                listView.ViewFields.Add(fieldName);
            }
            listView.Update();
        }
        public static void ModifyView(SPView view, IEnumerable<string> ViewFields, string query)
        {
            // Validation
            view.RequireNotNull("view");

            ViewFields.RequireNotNull("ViewFields");
            ViewFields.RequireNotEmpty("ViewFields");
            query.RequireNotNullOrEmpty("query");

            view.Query = query;

            view.ViewFields.DeleteAll();
            foreach (string fieldName in ViewFields)
            {
                view.ViewFields.Add(fieldName);
            }
            view.Update();
        }
        public static SPWorkflowTemplate GetWorkflowByName(SPWeb web, string workflowName)
        {
            // validation
            web.RequireNotNull("web");
            workflowName.RequireNotNullOrEmpty("workflowName");
            web.AllowUnsafeUpdates = true;
            SPWorkflowTemplate baseTemplate = (from SPWorkflowTemplate w in web.WorkflowTemplates
                                               where w.Name == workflowName
                                               select w).FirstOrDefault();

            return baseTemplate;
        }

        public static void AddWorkflow(SPWeb web, SPList list, SPList tasks, SPList workflowHistory, string workflowTemplateName, string workflowName)
        {
            // Validation
            web.RequireNotNull("web");
            list.RequireNotNull("list");
            tasks.RequireNotNull("tasks");
            workflowHistory.RequireNotNull("workflowHistory");
            workflowTemplateName.RequireNotNullOrEmpty("builtinWorkflowName");
            workflowName.RequireNotNull("workflowName");

            SPWorkflowTemplate approvalWorkflow = SharePointUtilities.GetWorkflowByName(web, workflowTemplateName);
            SPWorkflowAssociation association = SPWorkflowAssociation.CreateListAssociation(approvalWorkflow, workflowName, tasks, workflowHistory);
            association.AutoStartCreate = true;
            association.AutoStartChange = true;
            association.AllowManual = true;
            list.WorkflowAssociations.Add(association);
            list.Update();
        }

        public static SPFieldChoice CreateChoiceSiteColumn(SPWeb web, string fieldName, IEnumerable<string> choices, bool required)
        {
            // Validation
            web.RequireNotNull("web");
            fieldName.RequireNotNullOrEmpty("fieldName");
            choices.RequireNotNull("choices");
            choices.RequireNotEmpty("choices");

            if (web.AvailableFields.ContainsField(fieldName))
            {
                return web.AvailableFields[fieldName] as SPFieldChoice;
            }

            string internalName = web.Fields.Add(fieldName, SPFieldType.Choice, required);
            SPFieldChoice field = web.Fields.GetFieldByInternalName(internalName) as SPFieldChoice;

            if (null != field)
            {
                field.Choices.AddRange(choices.ToArray());
                field.Update();
            }
            return field;
        }

        public static SPView GetDefaultView(SPList list)
        {
            list.RequireNotNull("list");

            SPView listView = (from SPView v in list.Views
                               where v.DefaultView == true
                               select v).First();

            return listView;
        }

        public static void DeleteContentType(SPWeb web, string contentTypeName)
        {
            SPContentType faqContentType = web.AvailableContentTypes[contentTypeName];
            SPContentTypeId faqContentTypeID = faqContentType.Id;
            web.ContentTypes.Delete(faqContentTypeID);
        }

        public static void EnableContentApproval(SPList list)
        {
            list.DraftVersionVisibility = DraftVisibilityType.Approver;
            list.EnableModeration = true;
            list.Update();
        }

        public static void DefaultContentApproval(SPWeb web, SPList list, SPList tasks, SPList workflowHistory, string workflowName)
        {
            EnableContentApproval(list);
            const string builtInWorkflowName = "Approval - SharePoint 2010";
            web.RequireNotNull("web");
            list.RequireNotNull("list");
            tasks.RequireNotNull("tasks");
            workflowHistory.RequireNotNull("workflowHistory");
            workflowName.RequireNotNullOrEmpty("workflowName");
            web.AllowUnsafeUpdates = true;
            SPWorkflowTemplate approvalWorkflow = SharePointUtilities.GetWorkflowByName(web, builtInWorkflowName);
            SPWorkflowAssociation association = SPWorkflowAssociation.CreateListAssociation(approvalWorkflow, workflowName, tasks, workflowHistory);
            association.AutoStartCreate = true;
            association.AutoStartChange = true;
            association.AllowManual = true;
            string temp = association.AssociationData;
            XDocument associationData = XDocument.Parse(temp, LoadOptions.None);
            association.AssociationData = SharePointUtilities.GetDefaultAssociationData(web, associationData);
            list.WorkflowAssociations.Add(association);
        }

        public static string GetDefaultAssociationData(SPWeb web, XDocument associationData)
        {
            web.RequireNotNull("web");
            associationData.RequireNotNull("associationData");

            SPGroup defaultOwners = web.AssociatedOwnerGroup;
            XNamespace d = @"http://schemas.microsoft.com/office/infopath/2009/WSSList/dataFields";
            XNamespace pc = @"http://schemas.microsoft.com/office/infopath/2007/PartnerControls";

            XElement person = new XElement(pc + "Person",
                                new XElement(pc + "DisplayName", defaultOwners.Name),
                                new XElement(pc + "AccountId", defaultOwners.LoginName),
                                new XElement(pc + "AccountType", "SharePointGroup")
                                );

            associationData.Descendants(d + "Assignee").First().Add(person);
            associationData.Descendants(d + "AssignmentType").First().Value = "Parallel";
            associationData.Descendants(d + "NotificationMessage").First().Value = "Please approve this item!";
            var rejection = associationData.Descendants(d + "CancelonRejection").First();
            var change = associationData.Descendants(d + "CancelonChange").First();
            var enableContentApproval = associationData.Descendants(d + "EnableContentApproval").First();
            rejection.Value = change.Value = enableContentApproval.Value = "true";

            return associationData.ToString(SaveOptions.DisableFormatting);
        }

        public static void CopyStreams(Stream source, Stream destination)
        {
            byte[] buffer = new byte[64 * 1024];
            int bytesRead = source.Read(buffer, 0, buffer.Length);
            while (bytesRead != 0)
            {
                destination.Write(buffer, 0, buffer.Length);
                bytesRead = source.Read(buffer, 0, buffer.Length);
            }
        }

        public static SPFile UploadStream(SPDocumentLibrary library, Stream stream, string fileName)
        {
            library.RequireNotNull("library");
            stream.RequireNotNull("stream");
            fileName.RequireNotNullOrEmpty("fileName");
            SPFolder rootFolder = library.RootFolder;
            SPFile spFile = rootFolder.Files.Add(fileName, stream);
            library.Update();
            return spFile;
        }

        public static SPFile UploadFromPath(SPDocumentLibrary library, string path, string fileName)
        {
            library.RequireNotNull("library");
            path.RequireNotNullOrEmpty("path");
            fileName.RequireNotNullOrEmpty("fileName");
            using (Stream stream = File.OpenRead(path))
            {
                return UploadStream(library, stream, fileName);
            }
            
        }

        public static void AddPageToNavigation(SPWeb web, SPFile page, string navTitle)
        {
            web.RequireNotNull("web");
            page.RequireNotNull("page");
            navTitle.RequireNotNullOrEmpty("navTitle");

            // update the navigation
            SPNavigationNode node = new SPNavigationNode(navTitle, page.Url);
            // navigation is shared update the root
            if (web.ParentWeb.Navigation.UseShared)
            {
                using (SPSite site = new SPSite(web.Url))
                {
                    SPWeb rootWeb = site.RootWeb;
                    rootWeb.Navigation.TopNavigationBar.AddAsLast(node);
                }
            }

            else
            {
                web.Navigation.TopNavigationBar.AddAsLast(node);
            }
        }

        public static void AddWebPart(SPLimitedWebPartManager webPartManager, System.Web.UI.WebControls.WebParts.WebPart webPart, string zone, int zoneIndex, PartChromeType chromeType, string accesskey)
        {
            webPartManager.RequireNotNull("webPartManager");
            webPart.RequireNotNull("webPart");
            zone.RequireNotNullOrEmpty("zone");
            zoneIndex.Require(zoneIndex >= 0, "zoneIndex");


            webPart.AccessKey = accesskey;
            webPart.ChromeType = chromeType;
            webPartManager.AddWebPart(webPart, zone, zoneIndex);
            webPartManager.SaveChanges(webPart);
        }

        public static void ChangeTitleDisplayName(SPList list, string newTitle)
        {
            list.RequireNotNull("list");
            newTitle.RequireNotNullOrEmpty("newTitle");

            SPField titleField = list.Fields.TryGetFieldByStaticName("Title");
            if (null != titleField)
            {
                titleField.Title = newTitle;
                titleField.Update();
            }
        }

        public static void ChangeTitleDisplayName(SPContentType contentType, string newTitle)
        {
            contentType.RequireNotNull("contentType");
            newTitle.RequireNotNullOrEmpty("newTitle");

            SPFieldLink titleField = (from SPFieldLink field in contentType.FieldLinks
                                      where field.DisplayName.Equals("Title")
                                      select field).FirstOrDefault();
            if (null != titleField)
            {
                titleField.DisplayName = newTitle;
                contentType.Update(true); ;
            }
        }

        public static SPFeature ActivateFeatureIfNecessary(SPSite site, Guid featureGuid)
        {
            site.RequireNotNull("site");
            featureGuid.Require(Guid.Empty != featureGuid, "featureGuid");

            SPFeature feature = site.Features[featureGuid];
            if (null == feature)
            {
                feature = site.Features.Add(featureGuid);
            }

            return feature;
        }

        public static SPFeature ActivateFeatureIfNecessary(SPWeb web, Guid featureGuid)
        {
            web.RequireNotNull("web");
            featureGuid.Require(Guid.Empty != featureGuid, "featureGuid");

            SPFeature feature = web.Features[featureGuid];
            if (null == feature)
            {
                feature = web.Features.Add(featureGuid);
            }

            return feature;
        }

        public static SPUserSolution AddSandboxedSolution(SPSite site, byte[] fileData, string solutionName)
        {
            site.RequireNotNull("site");
            fileData.RequireNotNull("fileData");
            solutionName.RequireNotNullOrEmpty("solutionName");

            SPDocumentLibrary solutionGallery = site.GetCatalog(SPListTemplateType.SolutionCatalog) as SPDocumentLibrary;
            if (null != solutionGallery)
            {
                solutionGallery.ParentWeb.AllowUnsafeUpdates = true;
                string solutionPath = Path.Combine(solutionGallery.RootFolder.ServerRelativeUrl, solutionName);
                if (!site.RootWeb.GetFile(solutionPath).Exists)
                {
                    SPFile solutionFile = solutionGallery.RootFolder.Files.Add(solutionName, fileData);
                    SPUserSolution solution = site.Solutions.Add(solutionFile.Item.ID);
                    return solution;
                }
            }

            return null;
        }

        public static SPUserSolution AddSandboxedSolution(SPSite site, string path, string solutionName)
        {
            site.RequireNotNull("site");
            path.RequireNotNullOrEmpty("path");
            solutionName.RequireNotNullOrEmpty("solutionName");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found!", path);
            }
            return AddSandboxedSolution(site, File.ReadAllBytes(path), solutionName);
        }

        public static TaxonomyField CreateMangedMetadataSiteColumn(SPWeb web, string fieldName, TermSet termSet, string GroupName)
        {
            web.RequireNotNull("web");
            fieldName.RequireNotNullOrEmpty("fieldName");
            termSet.RequireNotNull("termSet");

            if (web.Fields.ContainsField(fieldName))
            {
                return web.Fields[fieldName] as TaxonomyField;
            }

            TaxonomyField field = web.Fields.CreateNewField("TaxonomyFieldType", fieldName) as TaxonomyField;
            field.SspId = termSet.TermStore.Id;
            field.TermSetId = termSet.Id;
            field.TargetTemplate = string.Empty;
            ///field.AllowMultipleValues = true;
            // field.CreateValuesInEditForm = true;
            field.Open = true;
            field.AnchorId = Guid.Empty;
            field.Group = !string.IsNullOrEmpty(GroupName) ? GroupName : "Managed Metadata";
            web.Fields.Add(field);
            web.Update();
            return web.Fields[fieldName] as TaxonomyField;
        }

        public static SPFeature ActivateFeatureIfNecessary(SPWeb web, Guid featureGuid, bool force, SPFeatureDefinitionScope sPFeatureDefinitionScope)
        {
            web.RequireNotNull("web");
            featureGuid.Require(Guid.Empty != featureGuid, "featureGuid");

            SPFeature feature = web.Features[featureGuid];
            if (null == feature || force)
            {
                feature = web.Features.Add(featureGuid, force, sPFeatureDefinitionScope);
            }

            return feature;
        }

        public static SPFeature ActivateFeatureIfNecessary(SPSite site, Guid featureGuid, bool force, SPFeatureDefinitionScope sPFeatureDefinitionScope)
        {
            site.RequireNotNull("site");
            featureGuid.Require(Guid.Empty != featureGuid, "featureGuid");

            SPFeature feature = site.Features[featureGuid];
            if (null == feature || force)
            {
                feature = site.Features.Add(featureGuid, force, sPFeatureDefinitionScope);
            }

            return feature;
        }

        public static SPContentType TryFindContentType(SPWeb rootWeb, string contentTypeName)
        {
            rootWeb.RequireNotNull("rootWeb");
            contentTypeName.RequireNotNullOrEmpty("contentTypeName");
            SPContentType bestPracticesContentType = (from SPContentType t in rootWeb.ContentTypes
                                                      where t.Name.Equals(contentTypeName)
                                                      select t).FirstOrDefault();
            return bestPracticesContentType;
        }

        public static SPField TryGetField(SPFieldCollection siteColumns, string fieldName)
        {
            siteColumns.RequireNotNull("siteColumns");
            fieldName.RequireNotNullOrEmpty("fieldName");

            return siteColumns.ContainsField(fieldName) ? siteColumns.GetField(fieldName) : null;
        }

        public static void AddFieldToContentType(SPContentType cType, SPField field)
        {
            cType.RequireNotNull("cType");
            field.RequireNotNull("field");
            var matchingLinks = from SPFieldLink fl in cType.FieldLinks
                                where fl.DisplayName.Equals(field.Title)
                                select fl;
            if (!cType.Fields.Contains(field.Id) && matchingLinks.Count() == 0)
            {
                SPFieldLink fieldLink = new SPFieldLink(field);
                cType.FieldLinks.Add(fieldLink);
                cType.Update(true);
            }
        }

        public static void AddFieldToContentType(SPContentType cType, IEnumerable<SPField> fields)
        {
            cType.RequireNotNull("cType");
            fields.RequireNotNull("field");

            foreach (SPField field in fields)
            {
                var matchingLinks = from SPFieldLink fl in cType.FieldLinks
                                    where fl.DisplayName.Equals(field.Title)
                                    select fl;
                if (!cType.Fields.Contains(field.Id) && matchingLinks.Count() == 0)
                {
                    SPFieldLink fieldLink = new SPFieldLink(field);
                    cType.FieldLinks.Add(fieldLink);
                }
            }

            cType.Update(true);
        }

        public static void AddFieldToList(SPList list, SPField field)
        {
            list.RequireNotNull("list");
            field.RequireNotNull("field");

            if (!list.Fields.Contains(field.Id))
            {
                list.Fields.Add(field);
                list.Update();
            }
        }

        public static void AddFieldToList(SPList list, IEnumerable<SPField> fields)
        {
            list.RequireNotNull("list");
            fields.RequireNotNull("field");

            foreach (SPField field in fields)
            {
                if (null != field && !list.Fields.Contains(field.Id))
                {
                    list.Fields.Add(field);
                }
            }

            list.Update();
        }

        public static Term CreateTerm(Term term, Guid termGuid, string nameString, int LCID)
        {
            term.RequireNotNull("term");
            nameString.RequireNotNullOrEmpty("nameString");

            Term newTerm = null;
            if (!Guid.Empty.Equals(termGuid))
            {
                newTerm = term.CreateTerm(nameString, LCID, termGuid);
            }
            else
            {
                newTerm = term.CreateTerm(nameString, LCID);
            }
            return newTerm;
        }

        public static Term CreateTerm(TermSet termSet, Guid termGuid, string nameString, int LCID)
        {
            termSet.RequireNotNull("termSet");
            nameString.RequireNotNullOrEmpty("nameString");
            Term newTerm = null;
            if (!Guid.Empty.Equals(termGuid))
            {
                newTerm = termSet.CreateTerm(nameString, LCID, termGuid);
            }
            else
            {
                newTerm = termSet.CreateTerm(nameString, LCID);
            }
            return newTerm;
        }

        public static Term FindTerm(TermSet termSet, Guid termGuid, string nameString)
        {
            termSet.RequireNotNull("termSet");
            nameString.RequireNotNullOrEmpty("nameString");
            Term term = termSet.Terms.FirstOrDefault(t => t.Id.Equals(termGuid) || t.Name.Equals(nameString));
            return term;
        }

        public static Term FindTerm(Term term, Guid termGuid, string nameString)
        {
            term.RequireNotNull("term");
            nameString.RequireNotNullOrEmpty("nameString");
            Term nestedTerm = term.Terms.FirstOrDefault(t => t.Id.Equals(termGuid) || t.Name.Equals(nameString));
            return nestedTerm;
        }

        public static void TraceDebugException(string ErrorMessage, Type objType, Exception exception)
        {
            WriteTraceDebugMessage(ErrorMessage, objType, exception);
        }
        static UInt32 ExceptionUID = 0;
        static UInt32 InfoUID = 0;
        private static void WriteTraceDebugMessage(string ErrorMessage, Type objType, Exception exception)
        {

            string output = string.Format("Type: {0} {1}\r\nException: {2}", objType, ErrorMessage, exception);
            SPSecurity.RunWithElevatedPrivileges(() => LoggingService.Current.WriteTrace(ExceptionUID++, LoggingService.DefaultErrorCategory, TraceSeverity.High, output, null));
            Trace.TraceInformation(output);
            Debug.WriteLine(output);
        }

        public static void TraceDebugInformation(string MethodMessage, Type objType)
        {
            WriteTraceDebugMessage(MethodMessage, objType);
        }

        private static void WriteTraceDebugMessage(string MethodMessage, Type objType)
        {
            string output = string.Format("Type: {0} Message: {1}", objType, MethodMessage);
            SPSecurity.RunWithElevatedPrivileges(() => LoggingService.Current.WriteTrace(InfoUID++, LoggingService.DefaultCategory, TraceSeverity.Monitorable, output, null));
            Trace.TraceInformation(output);
            Debug.WriteLine(output);
        }

        public static void ReorderContentTypeFields(SPContentType contentType, IEnumerable<string> fields)
        {
            contentType.RequireNotNull("contentType");
            fields.RequireNotNull("fields");
            fields.RequireNotEmpty("fields");

            List<string> staticNames = new List<string>();
            foreach (var fieldName in fields)
            {
                if (!string.IsNullOrEmpty(fieldName))
                {
                    SPField field = SharePointUtilities.TryGetField(contentType.Fields, fieldName);
                    SPFieldLink fieldLink = (from SPFieldLink f in contentType.FieldLinks
                                            where f.DisplayName.Equals(fieldName) || f.Name.Equals(fieldName)
                                            select f).FirstOrDefault();
                    if (null != field || null != fieldLink)
                    {
                        staticNames.Add( null != field ? field.StaticName : fieldLink.Name);
                    }
                    else
                    {
                        TraceDebugInformation(string.Format("{0} field is null!", fieldName), typeof(SharePointUtilities));
                    }
                }
            }

            contentType.FieldLinks.Reorder(staticNames.ToArray());
            contentType.Update(true);
        }

        public static void AddWorkflow(SPWeb web, SPContentType contentType, SPList tasks, SPList workflowHistory, string workflowTemplateName, string workflowName)
        {
            // Validation
            web.RequireNotNull("web");
            contentType.RequireNotNull("list");
            tasks.RequireNotNull("tasks");
            workflowHistory.RequireNotNull("workflowHistory");
            workflowTemplateName.RequireNotNullOrEmpty("workflowTemplateName");
            workflowName.RequireNotNull("workflowName");

            SPWorkflowTemplate workflowtemplate = SharePointUtilities.GetWorkflowByName(web, workflowTemplateName);
            SPWorkflowAssociation association = SPWorkflowAssociation.CreateWebContentTypeAssociation(workflowtemplate, workflowName, tasks.Title, workflowHistory.Title);
            association.AutoStartCreate = true;
            association.AutoStartChange = true;
            association.AllowManual = true;
            if (null == contentType.WorkflowAssociations.GetAssociationByName(association.Name, web.UICulture))
            {
                contentType.WorkflowAssociations.Add(association);
            }

            contentType.UpdateWorkflowAssociationsOnChildren(true,  // Do not generate full change list
                                                                 true,   // Push down to derived content types
                                                                 true,   // Push down to list content types
                                                                 false); // Do not throw exception if sealed or readonly  

        }

        /// <summary>
        /// AddWorkflow with 
        /// </summary>
        /// <param name="web"></param>
        /// <param name="contentType"></param>
        /// <param name="tasks"></param>
        /// <param name="workflowHistory"></param>
        /// <param name="workflowTemplateName"></param>
        /// <param name="workflowName"></param>
        /// <param name="associationData"></param>
        public static void AddWorkflow(SPWeb web, SPContentType contentType, SPList tasks, SPList workflowHistory, string workflowTemplateName, string workflowName, object associationData)
        {
            // Validation
            web.RequireNotNull("web");
            contentType.RequireNotNull("list");
            tasks.RequireNotNull("tasks");
            workflowHistory.RequireNotNull("workflowHistory");
            workflowTemplateName.RequireNotNullOrEmpty("workflowTemplateName");
            workflowName.RequireNotNull("workflowName");
            associationData.RequireNotNull("associationData");

            SPWorkflowTemplate workflowtemplate = SharePointUtilities.GetWorkflowByName(web, workflowTemplateName);
            SPWorkflowAssociation association = SPWorkflowAssociation.CreateWebContentTypeAssociation(workflowtemplate, workflowName, tasks.Title, workflowHistory.Title);
            association.AutoStartCreate = true;
            association.AutoStartChange = true;
            association.AllowManual = true;
            XmlSerializer serializer = new XmlSerializer(associationData.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, associationData);
                stream.Position = 0;
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                association.AssociationData = Encoding.UTF8.GetString(bytes);
            }
            if (null == contentType.WorkflowAssociations.GetAssociationByName(association.Name, web.UICulture))
            {
                contentType.WorkflowAssociations.Add(association);
            }

            contentType.UpdateWorkflowAssociationsOnChildren(true,  // Do not generate full change list
                                                                 true,   // Push down to derived content types
                                                                 true,   // Push down to list content types
                                                                 false); // Do not throw exception if sealed or readonly  

        }

        public static void AddEventReceiverToContentType(string className, SPContentType contentType, string assemblyName, SPEventReceiverType eventReceiverType, SPEventReceiverSynchronization eventReceiverSynchronization)
        {
            className.RequireNotNullOrEmpty("className");
            contentType.RequireNotNull("contentType");
            assemblyName.RequireNotNullOrEmpty("assemblyName");
            eventReceiverType.RequireNotNull("eventReceiverType");
            eventReceiverSynchronization.RequireNotNull("eventReceiverSynchronization");

            SPEventReceiverDefinition eventReceiver = contentType.EventReceivers.Add();
            eventReceiver.Synchronization = eventReceiverSynchronization;
            eventReceiver.Type = eventReceiverType;
            eventReceiver.Assembly = assemblyName;
            eventReceiver.Class = className;
            eventReceiver.Update();
        }
        public static void DeleteList(SPWeb web, string ListTitle)
        {
            web.RequireNotNull("web");
            ListTitle.RequireNotNullOrEmpty("ListTitle");

            SPList list = web.Lists.TryGetList(ListTitle);
            if (null != list)
            {
                web.Lists.Delete(list.ID);
            }
        }
        public static void SetDefaultTermValue(TermSet termSet, TaxonomyField field, string defaultTermValue)
        {
            Term defaultTerm = termSet.Terms[defaultTermValue];
            if (null != defaultTerm)
            {
                field.DefaultValue = string.Format("1;#{0}|{1}", defaultTerm.Name, defaultTerm.Id);
            }
            field.Update(true);
        }

        public static void SetDefaultTermValue(Group group, TaxonomyField field, string defaultTermText)
        {
            TermSet termSet = group.TermSets[field.TermSetId];
            if (null != termSet)
            {
                SetDefaultTermValue(termSet, field, defaultTermText);
            }
        }

        public static string DocIconPath = "TEMPLATE\\XML\\Docicon.xml";
        public static XElement DocIconXML
        {
            get
            {

                XElement featuresXml = null;
                if (!string.IsNullOrEmpty(DocIconPath))
                {
                    // Construct the path from the SharePoint root folder to
                    // the file specified in the webtemp
                    string path = SPUtility.GetGenericSetupPath(Path.GetDirectoryName(DocIconPath));
                    path = Path.Combine(path, Path.GetFileName(DocIconPath));

                    // Load the xml file
                    featuresXml = XElement.Load(path);
                }

                return featuresXml;
            }
        }

        public static string GetIconPath(string fileExt)
        {
            fileExt.RequireNotNullOrEmpty("fileExt");
            var ByExtensionElement = DocIconXML.Element("ByExtension");
            if (null != ByExtensionElement)
            {
                var mappingElement = (from m in ByExtensionElement.Elements("Mapping")
                                      where m.Attribute("Key") != null && m.Attribute("Key").Value != null && m.Attribute("Key").Value.Equals(fileExt)
                                      select m.Attribute("Value")).FirstOrDefault();

                if (null != mappingElement && !string.IsNullOrEmpty(mappingElement.Value))
                {
                    return string.Format("\\_layouts\\images\\{0}", mappingElement.Value);
                }
            }

            return "\\_layouts\\images\\ICGEN.GIF";
        }

        public static SPList CreateWiki(SPWeb web, string title, string description)
        {
            web.RequireNotNull("web");
            title.RequireNotNullOrEmpty("title");
            description.RequireNotNullOrEmpty("description");

            SPListTemplate template = web.ListTemplates["Wiki Page Library"];
            Guid listID = new Guid();
            listID = web.Lists.Add(title, description, template);
            SPList list = web.Lists[listID];
            list.OnQuickLaunch = true;
            list.Update();

            return list;
        }

        public static SPList CreateLibraryFromNamedTemplate(SPWeb web, string title, string description, string templateName)
        {
            web.RequireNotNull("web");
            title.RequireNotNullOrEmpty("title");
            description.RequireNotNullOrEmpty("description");
            templateName.RequireNotNullOrEmpty("templateName");            
            SPListTemplate template = web.ListTemplates[templateName];
            Guid listID = new Guid();
            listID = web.Lists.Add(title, description, template);
            SPList list = web.Lists[listID];
            list.OnQuickLaunch = true;
            list.Update();

            return list;
        }

        public static string LoadFromSharePointRoot(string relativePath)
        {
            relativePath.RequireNotNullOrEmpty("relativePath");
            string path = SPUtility.GetGenericSetupPath(Path.GetDirectoryName(relativePath));
            path = Path.Combine(path, Path.GetFileName(relativePath));
            return path;
        }

        public static SPFile CreatePage(SPList list, string fileName, SPTemplateFileType fileType)
        {
            list.RequireNotNull("list");
            fileName.RequireNotNullOrEmpty("fileName");
            return list.RootFolder.Files.Add(string.Format("{0}/{1}", list.RootFolder.ServerRelativeUrl, fileName), fileType);
        }

        public static void ConnectListViewWebParts(SPLimitedWebPartManager webPartManager, ListViewWebPart providerWebPart, ListViewWebPart consumerWebPart, SPRowToParametersTransformer transformer, string consumerInternalFieldName, string providerInternalFieldName)
        {
            webPartManager.RequireNotNull("webPartManager");
            providerWebPart.RequireNotNull("providerWebPart");
            consumerWebPart.RequireNotNull("consumerWebPart");
            transformer.RequireNotNull("transformer");
            consumerInternalFieldName.RequireNotNullOrEmpty("consumerInternalFieldName");
            providerInternalFieldName.RequireNotNullOrEmpty("providerInternalFieldName");

            ProviderConnectionPoint providerConnectionPoint = (from ProviderConnectionPoint conn in webPartManager.GetProviderConnectionPoints(providerWebPart)
                                                               where String.Equals("Provide Row To", conn.DisplayName, StringComparison.OrdinalIgnoreCase) && conn.InterfaceType == typeof(IWebPartRow)
                                                               select conn).FirstOrDefault();
            ConsumerConnectionPoint consumerConnectionPoint = (from ConsumerConnectionPoint conn in webPartManager.GetConsumerConnectionPoints(consumerWebPart)
                                                               where String.Equals("Get Sort/Filter From", conn.DisplayName, StringComparison.OrdinalIgnoreCase) && conn.InterfaceType == typeof(IWebPartParameters)
                                                               select conn).FirstOrDefault();

            consumerWebPart.Connections = consumerWebPart.ConnectionID + "," + providerWebPart.ConnectionID + "," +
                                             consumerConnectionPoint.ID + "," + providerConnectionPoint.ID + "," +
                                             consumerConnectionPoint.ID + "," + providerConnectionPoint.ID + "," +
                                              consumerInternalFieldName + "=" + providerInternalFieldName;

            webPartManager.SaveChanges(consumerWebPart);
        }

        public static void RemoveTimerJob(SPWebApplication WebApplication, string timerJobName)
        {
            var matchingJobs = from SPJobDefinition job in WebApplication.JobDefinitions
                               where job.Name.Equals(timerJobName)
                               select job;
            foreach (var job in matchingJobs)
            {
                job.Delete();
            }
        }

        public static SPUser GetSPUser(SPListItem item, string key)
        {
            SPFieldUser field = item.Fields[key] as SPFieldUser;
            if (field != null)
            {
                SPFieldUserValue fieldValue =
                 field.GetFieldValue(item[key].ToString()) as SPFieldUserValue;
                if (fieldValue != null)
                    return fieldValue.User;
            }

            return null;
        }

        public static string GetPickerEntities(PeopleEditor Editor, char separator)
        {
            Editor.RequireNotNull("Editor");
            if (Editor.Entities.Count == 0)
            {
                return string.Empty;
            }
            PickerEntity firstEntity = (PickerEntity)Editor.Entities[0];
            StringBuilder sb = new StringBuilder(firstEntity.Key);
            foreach (PickerEntity entity in Editor.Entities.OfType<PickerEntity>().Skip(1))
            {
                sb.AppendFormat("{0}{1}", separator, entity.Key);
            }
            return sb.ToString();
        }

    }
}
