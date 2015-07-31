using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint.Administration;

namespace MySP2010Utilities
{
    public static class ServiceLocationRegistration
    {
        public static void Register()
        {
            SPSecurity.RunWithElevatedPrivileges(RegisterTypes);
        }
        public static void Register(SPWeb web)
        {
            web.RequireNotNull("web");
            elevatedPrivilegesRegisterTypes(web);
        }

        public static void Register(SPSite site)
        {
            site.RequireNotNull("site");

            elevatedPrivilegesRegisterTypes(site);
        }

        private static void elevatedPrivilegesRegisterTypes(SPSite site)
        {
            SPSecurity.RunWithElevatedPrivileges(() => RegisterTypes(site));
        }

        private static void elevatedPrivilegesRegisterTypes(SPWeb web)
        {
            SPSecurity.RunWithElevatedPrivileges(() => RegisterTypes(web));
        }

        public static void Unregister(SPSite site)
        {
            site.RequireNotNull("site");
            elevatedPrivilegesUnRegisterTypes(site);
        }

        public static void Unregister(SPWeb web)
        {
            web.RequireNotNull("web");
            elevatedPrivilegesUnRegisterTypes(web);
        }

        public static void Unregister()
        {
            SPSecurity.RunWithElevatedPrivileges(UnregisterTypes);
        }

        private static void elevatedPrivilegesUnRegisterTypes(SPSite site)
        {
            SPSecurity.RunWithElevatedPrivileges(() => UnregisterTypes(site));
        }

        private static void elevatedPrivilegesUnRegisterTypes(SPWeb web)
        {
            SPSecurity.RunWithElevatedPrivileges(() => UnregisterTypes(web));
        }


        private static void RegisterTypes()
        {
            LogUtility logger = new LogUtility();
            try
            {
                logger.TraceDebugInformation("Registering types for use across farms", typeof(ServiceLocationRegistration));
                IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
                IServiceLocatorConfig typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();
                registerTypeMappings(typeMappings);
            }
            catch (Exception exception)
            {
                logger.TraceDebugException("Exception while registering types!", typeof(ServiceLocationRegistration), exception);
            }
            finally
            {
                logger.TraceDebugInformation("Finished registering types for use across farms", typeof(ServiceLocationRegistration));
            }
        }

        private static void RegisterTypes(SPWeb web)
        {
            web.RequireNotNull("web");
            LogUtility logger = new LogUtility();
            try
            {
                logger.TraceDebugInformation("Registering types for use across farms", typeof(ServiceLocationRegistration));
                IServiceLocator serviceLocator = new SPWebServiceLocator(web);
                IServiceLocatorConfig typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();
                registerTypeMappings(typeMappings);
            }
            catch (Exception exception)
            {
                logger.TraceDebugException("Exception while registering types!", typeof(ServiceLocationRegistration), exception);
            }
            finally
            {
                logger.TraceDebugInformation("Finished registering types for use across farms", typeof(ServiceLocationRegistration));
            }
        }


        private static void RegisterTypes(SPSite site)
        {
            site.RequireNotNull("site");
            LogUtility logger = new LogUtility();
            try
            {
                logger.TraceDebugInformation("Registering types for use across farms", typeof(ServiceLocationRegistration));
                IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
                IServiceLocatorConfig typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();
                typeMappings.Site = site;
                registerTypeMappings(typeMappings);
            }
            catch (Exception exception)
            {
                logger.TraceDebugException("Exception while registering types!", typeof(ServiceLocationRegistration), exception);
            }
            finally
            {
                logger.TraceDebugInformation("Finished registering types for use across farms", typeof(ServiceLocationRegistration));
            }
        }

        private static void registerTypeMappings(IServiceLocatorConfig typeMappings)
        {
            LogUtility logger = new LogUtility();
            logger.TraceDebugInformation("starting registerTypeMappings", typeof(ServiceLocationRegistration));
            typeMappings.RegisterTypeMapping<IServiceLocatorConfig, ServiceLocatorConfig>();
            typeMappings.RegisterTypeMapping<IAddSandboxedSolutions, AddSandboxedSolutions>();
            typeMappings.RegisterTypeMapping<IContentTypeOperations, ContentTypeOperations>();
            typeMappings.RegisterTypeMapping<IDocIconOperations, DocIconOperations>();
            typeMappings.RegisterTypeMapping<IFeatureActivator, FeatureActivator>();
            typeMappings.RegisterTypeMapping<IFieldOperations, FieldOperations>();
            typeMappings.RegisterTypeMapping<IListOperations, ListOperations>();
            //typeMappings.RegisterTypeMapping<ILogUtility, SPListLogger>();
            typeMappings.RegisterTypeMapping<ILogUtility, LogUtility>();
            typeMappings.RegisterTypeMapping<IManagedMetaDataOperations, ManagedMetaDataOperations>();
            typeMappings.RegisterTypeMapping<IModifyContentType, ModifyContentType>();
            typeMappings.RegisterTypeMapping<IViewOperations, ModifyViewClass>();
            typeMappings.RegisterTypeMapping<INavigationCustomization, NavigationCustomization>();
            typeMappings.RegisterTypeMapping<ISiteColumnOperations, SiteColumnOperations>();
            typeMappings.RegisterTypeMapping<ISPFileOperations, SPFileOperations>();
            typeMappings.RegisterTypeMapping<ISPUserOperations, SPUserOperations>();
            typeMappings.RegisterTypeMapping<ITimerJobOperations, TimerJobOperations>();
            typeMappings.RegisterTypeMapping<IWebPartOperations, WebPartOperations>();
            typeMappings.RegisterTypeMapping<IWorkflowOperations, WorkflowOperations>();
            typeMappings.RegisterTypeMapping<IFaceBookGraphAPIOperations, FaceBookGraphAPIOperations>();
            typeMappings.RegisterTypeMapping<IWorkflowMessageParser, WorkflowMessageParser>();
            typeMappings.RegisterTypeMapping<IUserParser, UserParser>();
            typeMappings.RegisterTypeMapping<ISLAMConfigurationGenerator, SLAMConfigurationGenerator>();
            typeMappings.RegisterTypeMapping<IListTemplateFinder, ListTemplateFinder>();
            typeMappings.RegisterTypeMapping<IWikiPagesOperations, WikiPagesOperations>();
            typeMappings.RegisterTypeMapping<IContentOrganizerRuleCreationData, ContentOrganizerRuleCreationData>();
            typeMappings.RegisterTypeMapping<IContentOrganizerConditionalData, ContentOrganizerConditionalData>();
            typeMappings.RegisterTypeMapping<IContentOrganizerCreator, ContentOrganizerCreator>();
            typeMappings.RegisterTypeMapping<ICustomContentOrganizerRouterRegistrar, CustomContentOrganizerRouterRegistrar>();
            typeMappings.RegisterTypeMapping<ILinkToDocumentCreator, LinkToDocumentCreator>();
            logger.TraceDebugInformation("Finishing registerTypeMappings", typeof(ServiceLocationRegistration));
        }

        private static void UnregisterTypes()
        {
            LogUtility logUtility = new LogUtility();
            try
            {
                logUtility.TraceDebugInformation("Deactivating Local utilities", typeof(ServiceLocationRegistration));
                IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
                IServiceLocatorConfig typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();
                unregisterTypeMappings(typeMappings);
                logUtility.TraceDebugInformation("Successfully deactivated Local utilities", typeof(ServiceLocationRegistration));
            }
            catch (Exception exception)
            {
                logUtility.TraceDebugException("Error while deactivating Local utilities", typeof(ServiceLocationRegistration), exception);
            }

            finally
            {
                logUtility.TraceDebugInformation("Finished deactivating Local utilities", typeof(ServiceLocationRegistration));
            }
        }

        private static void UnregisterTypes(SPWeb web)
        {
            LogUtility logUtility = new LogUtility();
            try
            {
                logUtility.TraceDebugInformation("Deactivating Local utilities", typeof(ServiceLocationRegistration));
                IServiceLocator serviceLocator = new SPWebServiceLocator(web);
                IServiceLocatorConfig typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();
                unregisterTypeMappings(typeMappings);
                logUtility.TraceDebugInformation("Successfully deactivated Local utilities", typeof(ServiceLocationRegistration));
            }
            catch (Exception exception)
            {
                logUtility.TraceDebugException("Error while deactivating Local utilities", typeof(ServiceLocationRegistration), exception);
            }

            finally
            {
                logUtility.TraceDebugInformation("Finished deactivating Local utilities", typeof(ServiceLocationRegistration));
            }
        }
        
        private static void UnregisterTypes(SPSite site)
        {
            site.RequireNotNull("site");
            LogUtility logUtility = new LogUtility();
            try
            {
                logUtility.TraceDebugInformation("Deactivating Local utilities", typeof(ServiceLocationRegistration));
                IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
                IServiceLocatorConfig typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();
                typeMappings.Site = site;
                unregisterTypeMappings(typeMappings);
                logUtility.TraceDebugInformation("Successfully deactivated Local utilities", typeof(ServiceLocationRegistration));
            }
            catch (Exception exception)
            {
                logUtility.TraceDebugException("Error while deactivating Local utilities", typeof(ServiceLocationRegistration), exception);
            }

            finally
            {
                logUtility.TraceDebugInformation("Finished deactivating Local utilities", typeof(ServiceLocationRegistration));
            }
        }

        private static void unregisterTypeMappings(IServiceLocatorConfig typeMappings)
        {
            typeMappings.RemoveTypeMapping<IAddSandboxedSolutions>(null);
            typeMappings.RemoveTypeMapping<IContentTypeOperations>(null);
            typeMappings.RemoveTypeMapping<IDocIconOperations>(null);
            typeMappings.RemoveTypeMapping<IFeatureActivator>(null);
            typeMappings.RemoveTypeMapping<IFieldOperations>(null);
            typeMappings.RemoveTypeMapping<IListOperations>(null);
            typeMappings.RemoveTypeMapping<ILogUtility>(null);
            typeMappings.RemoveTypeMapping<IManagedMetaDataOperations>(null);
            typeMappings.RemoveTypeMapping<IModifyContentType>(null);
            typeMappings.RemoveTypeMapping<IViewOperations>(null);
            typeMappings.RemoveTypeMapping<INavigationCustomization>(null);
            typeMappings.RemoveTypeMapping<ISiteColumnOperations>(null);
            typeMappings.RemoveTypeMapping<ISPFileOperations>(null);
            typeMappings.RemoveTypeMapping<ISPUserOperations>(null);
            typeMappings.RemoveTypeMapping<ITimerJobOperations>(null);
            typeMappings.RemoveTypeMapping<IWebPartOperations>(null);
            typeMappings.RemoveTypeMapping<IWorkflowOperations>(null);
            typeMappings.RemoveTypeMapping<IFaceBookGraphAPIOperations>(null);
            typeMappings.RemoveTypeMapping<IUserParser>(null);
            typeMappings.RemoveTypeMapping<ISLAMConfigurationGenerator>(null);
            typeMappings.RemoveTypeMapping<IListTemplateFinder>(null);
            typeMappings.RemoveTypeMapping<IWikiPagesOperations>(null);
            typeMappings.RemoveTypeMapping<IContentOrganizerRuleCreationData>(null);
            typeMappings.RemoveTypeMapping<IContentOrganizerConditionalData>(null);
            typeMappings.RemoveTypeMapping<IContentOrganizerCreator>(null);
            typeMappings.RemoveTypeMapping<ILinkToDocumentCreator>(null);
        }
    }
}
