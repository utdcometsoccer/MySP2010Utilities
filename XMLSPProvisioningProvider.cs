using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.SharePoint;
using System.Xml.Linq;
using Microsoft.SharePoint.Utilities;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;

namespace MySP2010Utilities
{
    /// <summary>
    /// You must set the ProvisionData property int the WebTemp xml file
    /// </summary>
    public class XMLSPProvisioningProvider : SPWebProvisioningProvider
    {
        public XMLSPProvisioningProvider()
        {
            serviceLocator = SharePointServiceLocator.GetCurrent();
            logger = serviceLocator.GetInstance<ILogUtility>();
        }
        private ILogUtility logger;
        IServiceLocator serviceLocator;
        private const string SITE_TEMPLATE = "STS#1";
        protected SPFile DefaultPage { get; set; }
        public override void Provision(SPWebProvisioningProperties props)
        {
            // Create a blank site to begin from
            props.Web.ApplyWebTemplate(SITE_TEMPLATE);

            // Save this so it is available in other methods
            Properties = props;

            SPSecurity.CodeToRunElevated code = new SPSecurity.CodeToRunElevated(CreateSite);
            SPSecurity.RunWithElevatedPrivileges(code);
        }

        #region Protected Methods

        /// <summary>
        /// Create the site
        /// </summary>
        protected virtual void CreateSite()
        {
            using (SPSite site = new SPSite(Properties.Web.Site.ID))
            {
                using (SPWeb web = site.OpenWeb(Properties.Web.ID))
                {
                    web.AllowUnsafeUpdates = true;
                    CustomPreWebCreation(site, web);
                    // Add specified features to this site
                    AddSiteFeatures(site);

                    // Add specified features to this web
                    AddWebFeatures(web);

                    // Add new default page
                    AddDefaultPage(web);

                    // Miscellanous page tasks
                    CustomPostWebCreation(site, web);
                }
            }
        }

        protected virtual void CustomPreWebCreation(SPSite site, SPWeb web)
        {

        }

        protected virtual void CustomPostWebCreation(SPSite site, SPWeb web)
        {

        }

        /// <summary>
        /// Add features to the given site
        /// </summary>
        /// <param name="site">SPSite to add features to</param>
        protected virtual void AddSiteFeatures(SPSite site)
        {
            List<XElement> features = (from f in DataFile.Elements("SiteFeatures")
                                .Elements("Feature")
                                       select f).ToList();

            foreach (XElement feature in features)
            {
                Guid featureID = new Guid(feature.Attribute("ID").Value);
                try
                {
                    SharePointUtilities.ActivateFeatureIfNecessary(site, featureID);
                }
                catch (Exception exception)
                {                    
                    logger.TraceDebugException(string.Format("Activation of site collection scoped feature with id: {0} failed!", featureID), GetType(), exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Add features to the given web
        /// </summary>
        /// <param name="web">SPWeb to add features to</param>
        protected virtual void AddWebFeatures(SPWeb web)
        {

            List<XElement> features = (from f in DataFile.Elements("WebFeatures")
                                  .Elements("Feature")
                                       select f).ToList();

            foreach (XElement feature in features)
            {
                Guid featureID = new Guid(feature.Attribute("ID").Value);
                try
                {
                    SharePointUtilities.ActivateFeatureIfNecessary(web, featureID);
                }
                catch (Exception exception)
                {
                    logger = new LogUtility();
                    logger.TraceDebugException(string.Format("Activation of web scoped feature with id: {0} failed!", featureID), GetType(), exception);
                    throw;
                }
            }

        }

        /// <summary>
        /// Add default page to site
        /// </summary>
        /// <param name="web">SPWeb to add page to</param>
        protected virtual void AddDefaultPage(SPWeb web)
        {
            logger.TraceDebugInformation(string.Format("Adding default page for web: {0} at url: {1}", web.Title, web.Url), GetType());
            string file = (from f in DataFile.Elements("DefaultPage")
                           select f).Single().Attribute("file").Value;

            string filePath = FeaturePath + "\\" + file;
            logger.TraceDebugInformation(string.Format("Createing default.aspx from file at path: {0}", filePath), GetType());
            TextReader reader = new StreamReader(filePath);
            
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            writer.Write(reader.ReadToEnd());
            writer.Flush();

            web.AllowUnsafeUpdates = true;
            DefaultPage = web.Files.Add("Default.aspx", outStream, true);
        }

        #endregion

        #region Properties

        protected SPWebProvisioningProperties Properties { get; set; }

        protected XElement DataFile
        {
            get
            {
                XElement featuresXml = null;
                if (Properties != null)
                {
                    // Construct the path from the SharePoint root folder to
                    // the file specified in the webtemp
                    string path = SPUtility.GetGenericSetupPath(Path.GetDirectoryName(Properties.Data));
                    //string path = SPUtility.GetVersionedGenericSetupPath(Path.GetDirectoryName(Properties.Data), 15);
                    path = Path.Combine(path, Path.GetFileName(Properties.Data));

                    // Load the xml file
                    featuresXml = XElement.Load(path);
                }

                return featuresXml;
            }
        }

        protected string FeaturePath
        {
            get
            {
                string path = string.Empty;
                if (Properties != null)
                {
                    
                    // Construct the path from the SharePoint root folder to
                    // the file specified in the webtemp
                    path = SPUtility.GetGenericSetupPath(Path.GetDirectoryName(Properties.Data));
                    //path = SPUtility.GetVersionedGenericSetupPath(Path.GetDirectoryName(Properties.Data), 15);
                    path = Path.GetDirectoryName(path);
                }
                return path;
            }
        }

        #endregion
    }
}
