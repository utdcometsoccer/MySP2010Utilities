using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;
using System.Xml.Linq;
using Microsoft.SharePoint;
using System.Xml;
using System.IO;

namespace MySP2010Utilities
{
    public class SLAMConfigurationGenerator : MySP2010Utilities.ISLAMConfigurationGenerator
    {

        UInt32 activateOrder = 1;
        List<Guid> ListIDs = new List<Guid>();
        /// <summary>
        /// The Byte array is the SLAM Configuration file
        /// </summary>
        /// <param name="webApplication"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public Byte[] WriteSlamConfig(SPWebApplication webApplication, string connectionString)
        {
            webApplication.RequireNotNull("webApplication");
            connectionString.RequireNotNullOrEmpty("connectionString");
            Byte[] SLAMConfigData = null;
            LogUtility logUtlity = new LogUtility();

            try
            {
                logUtlity.TraceDebugInformation("Writing SLAM Configuration now.", GetType());
                SLAMConfigData = WriteSlamConfigImpl(webApplication, connectionString);
            }
            catch (Exception exception)
            {
                logUtlity.TraceDebugException("Error while writing SLAM configuration!", GetType(), exception);
            }

            finally
            {
                logUtlity.TraceDebugInformation("Finished writing SLAM Configuration now.", GetType());
            }

            return SLAMConfigData;
        }

        private byte[] WriteSlamConfigImpl(SPWebApplication webApplication, string connectionString)
        {
            LogUtility logUtlity = new LogUtility();
            XDocument configurationFile = new XDocument();
            XElement configuration = new XElement("Configuration");
            configurationFile.Add(configuration);
            XElement connectionStrings = new XElement("ConnectionStrings");
            configuration.Add(connectionStrings);
            XAttribute defaultAttribute = new XAttribute("Default", "SLAM");
            connectionStrings.Add(defaultAttribute);
            connectionStrings.Add(new XElement("add",
                                                new XAttribute("Name", "SLAM"),
                                                new XAttribute("ConnectionString", connectionString)
                                                ));
            XElement dataMapping = new XElement("DataMapping", new XAttribute("DataSchema", "SLAM"));

            configuration.Add(dataMapping);
            SPWebApplication app = webApplication;
            if (null != app)
            {
                foreach (SPSite site in app.Sites)
                {
                    try
                    {
                        AddConfigInfo(dataMapping, site);
                    }
                    catch (Exception exception)
                    {
                        logUtlity.TraceDebugException("Error trying to write Config information", GetType(), exception);
                    }

                    finally
                    {
                        site.Dispose();
                    }
                }

                Byte[] slamConfigData = null;

                using (MemoryStream stream = new MemoryStream())
                {
                    XmlWriter xmlWriter = XmlWriter.Create(stream);
                    configurationFile.WriteTo(xmlWriter);
                    xmlWriter.Close();
                    slamConfigData = stream.ToArray();
                }
                logUtlity.TraceDebugInformation(string.Format("Writing {0} bytes of SLAM configuration data", slamConfigData.Length), GetType());
                return slamConfigData;
            }

            else
            {
                logUtlity.TraceDebugInformation("Cannot find Web Application at http://localhost!", GetType());
            }
            return null;
        }

        private void AddConfigInfo(XElement dataMapping, SPSite site)
        {
            var USCOContentTypes = from SPContentType c in site.RootWeb.ContentTypes
                                   where c.Group.Contains("USOC")
                                   select c;
            foreach (SPContentType contentType in USCOContentTypes)
            {
                XElement contentTypeElement = new XElement("ContentType",
                                            new XAttribute("Name", contentType.Name),
                                            new XAttribute("ActivationOrder", activateOrder++));
                SetFieldElements(dataMapping, site, contentType.Fields, contentTypeElement);
                var fieldsFromFieldLink = from SPFieldLink fieldLink in contentType.FieldLinks
                                          select site.RootWeb.Fields[fieldLink.Id];
                //SetFieldElements(dataMapping, site, fieldsFromFieldLink, contentTypeElement);
                dataMapping.Add(contentTypeElement);
            }
        }

        private void SetFieldElements(XElement dataMapping, SPSite site, SPFieldCollection fields, XElement contentTypeElement)
        {
            LogUtility logUtility = new LogUtility();
            foreach (SPField field in fields)
            {
                XElement fieldElement = new XElement("Field",
                                        new XAttribute("Name", field.Title),
                                        new XAttribute("Required", field.Required)
                        );

                switch (field.Type)
                {
                    case SPFieldType.Recurrence:
                        fieldElement.Add(new XAttribute("SqlType", "bit"), new XAttribute("SPType", "Recurrence"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.AllDayEvent:
                        fieldElement.Add(new XAttribute("SqlType", "bit"), new XAttribute("SPType", "AllDayEvent"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Boolean:
                        fieldElement.Add(new XAttribute("SqlType", "bit"), new XAttribute("SPType", "Boolean"));
                        contentTypeElement.Add(fieldElement);
                        break;

                    case SPFieldType.Calculated:
                        fieldElement.Add(new XAttribute("SqlType", "decimal"), new XAttribute("SPType", "Calculated"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.DateTime:
                        fieldElement.Add(new XAttribute("SqlType", "datetime"), new XAttribute("SPType", "DateTime"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.MultiChoice:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "MultiChoice"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.GridChoice:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "GridChoice"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Choice:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "Choice"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Integer:
                        fieldElement.Add(new XAttribute("SqlType", "int"), new XAttribute("SPType", "Integer"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Lookup:
                        SPFieldLookup lookupField = field as SPFieldLookup;
                        if (null != lookupField)
                        {
                            Guid webID = lookupField.LookupWebId;
                            try
                            {
                                ProcessLookupLists(dataMapping, site, logUtility, fieldElement, lookupField, webID, contentTypeElement);
                                
                            }
                            catch (Exception exception)
                            {
                                logUtility.TraceDebugException(string.Format("Caught exception trying to parse {0} field", field.Title), GetType(), exception);
                            }
                        }
                        break;
                    case SPFieldType.ModStat:
                        fieldElement.Add(new XAttribute("SqlType", "int"), new XAttribute("SPType", "ModStat"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Currency:
                        fieldElement.Add(new XAttribute("SqlType", "float"), new XAttribute("SPType", "Currency"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Number:
                        fieldElement.Add(new XAttribute("SqlType", "float"), new XAttribute("SPType", "Number"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.URL:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "Url"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Note:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(512)"), new XAttribute("SPType", "Note"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.User:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "User"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Computed:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "Computed"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Text:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "Text"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.WorkflowStatus:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "WorkflowStatus"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    default:
                        break;
                }
                logUtility.TraceDebugInformation(string.Format("Field Title: {0}, Field Type: {1}, Field class: {2}", field.Title, field.Type, field.GetType()), GetType());
            }
        }

        private void ProcessLookupLists(XElement dataMapping, SPSite site, LogUtility logUtility, XElement fieldElement, SPFieldLookup lookupField, Guid webID, XElement contentType)
        {            
            Guid listID = Guid.Empty;
            try
            {
                listID = new Guid(lookupField.LookupList);
            }
            catch (FormatException formatException)
            {
                logUtility.TraceDebugException(string.Format("Caught format Exception for field with id: {0} and title: {1}", lookupField.LookupField, lookupField.Title), GetType(), formatException);
            }
            
            if (!ListIDs.Contains(listID))
            {
                ListIDs.Add(listID);
                LookUpListProcess(dataMapping, site, logUtility, fieldElement, webID, listID, contentType);
            }
        }

        private void LookUpListProcess(XElement dataMapping, SPSite site, LogUtility logUtility, XElement fieldElement, Guid webID, Guid listID, XElement contentType)
        {
            using (SPWeb lookupWeb = site.AllWebs[webID])
            {
                SPList existingList = (from SPList l in lookupWeb.Lists
                                       where l.ID.Equals(listID)
                                       select l).FirstOrDefault();
                if (null != existingList)
                {
                    SPList lookupList = existingList;
                    fieldElement.Add(new XAttribute("SPType", "Lookup"),
                                    new XAttribute("AssociatedTypeName", "List"),
                                    new XAttribute("AssociationTableName", lookupList.Title));
                    contentType.Add(fieldElement);
                    string url = urlBuilder(lookupWeb);

                    logUtility.TraceDebugInformation(string.Format("Generating SLAM config for List with title: {0} on web at: {1}", lookupList.Title, url), GetType());
                    XElement LookUpListElement = new XElement("List",
                                                new XAttribute("Site", url),
                                                new XAttribute("Name", lookupList.Title),
                                                new XAttribute("ActivationOrder", activateOrder++));
                    SetFieldElements(dataMapping, site, lookupList.Fields, LookUpListElement);
                    dataMapping.Add(LookUpListElement);

                }
            }
        }

        private void SetFieldElements(XElement dataMapping, SPSite site, IEnumerable<SPField> fields, XElement contentTypeElement)
        {
            LogUtility logUtility = new LogUtility();
            foreach (SPField field in fields)
            {
                XElement fieldElement = new XElement("Field",
                                        new XAttribute("Name", field.Title),
                                        new XAttribute("Required", field.Required)
                        );

                switch (field.Type)
                {
                    case SPFieldType.Recurrence:
                        fieldElement.Add(new XAttribute("SqlType", "bit"), new XAttribute("SPType", "Recurrence"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.AllDayEvent:
                        fieldElement.Add(new XAttribute("SqlType", "bit"), new XAttribute("SPType", "AllDayEvent"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Boolean:
                        fieldElement.Add(new XAttribute("SqlType", "bit"), new XAttribute("SPType", "Boolean"));
                        contentTypeElement.Add(fieldElement);
                        break;

                    case SPFieldType.Calculated:
                        fieldElement.Add(new XAttribute("SqlType", "decimal"), new XAttribute("SPType", "Calculated"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.DateTime:
                        fieldElement.Add(new XAttribute("SqlType", "datetime"), new XAttribute("SPType", "DateTime"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.MultiChoice:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "MultiChoice"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.GridChoice:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "GridChoice"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Choice:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "Choice"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Integer:
                        fieldElement.Add(new XAttribute("SqlType", "int"), new XAttribute("SPType", "Integer"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Lookup:
                        SPFieldLookup lookupField = field as SPFieldLookup;
                        if (null != lookupField)
                        {
                            Guid webID = lookupField.LookupWebId;
                            try
                            {
                                ProcessLookupLists(dataMapping, site, logUtility, fieldElement, lookupField, webID, contentTypeElement);
                            }
                            catch (Exception exception)
                            {
                                logUtility.TraceDebugException(string.Format("Caught exception trying to parse {0} field", field.Title), GetType(), exception);
                            }
                        }
                        break;
                    case SPFieldType.ModStat:
                        fieldElement.Add(new XAttribute("SqlType", "int"), new XAttribute("SPType", "ModStat"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Currency:
                        fieldElement.Add(new XAttribute("SqlType", "float"), new XAttribute("SPType", "Currency"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Number:
                        fieldElement.Add(new XAttribute("SqlType", "float"), new XAttribute("SPType", "Number"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.URL:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "Url"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Note:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(512)"), new XAttribute("SPType", "Note"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.User:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "User"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Computed:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "Computed"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.Text:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "Text"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    case SPFieldType.WorkflowStatus:
                        fieldElement.Add(new XAttribute("SqlType", "nvarchar(255)"), new XAttribute("SPType", "WorkflowStatus"));
                        contentTypeElement.Add(fieldElement);
                        break;
                    default:
                        break;
                }

            }
        }

        private string urlBuilder(SPWeb lookupWeb)
        {
            LogUtility logUtility = new LogUtility();
            SPWeb currentWeb = lookupWeb;
            List<SPWeb> webs = new List<SPWeb>();
            bool lastWebWasRoot = false;
            while (!lastWebWasRoot)
            {
                logUtility.TraceDebugInformation(string.Format("Last web was not the root! Current Web is {0}", currentWeb), GetType());
                webs.Add(currentWeb);
                lastWebWasRoot = currentWeb.IsRootWeb;
                currentWeb = currentWeb.ParentWeb;
            }
            webs.Reverse();

            StringBuilder urlBuilder = new StringBuilder(webs.First().Title);
            foreach (SPWeb web in webs.Skip(1))
            {
                urlBuilder.AppendFormat("//{0}", web.Title);
            }

            return urlBuilder.ToString();
        }
    }
}
