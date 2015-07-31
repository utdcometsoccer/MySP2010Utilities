using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySP2010Utilities
{
    /// <summary>
    /// SPListLogger
    /// SharePoint List Logger.
    /// Creates lists named Utilities_Errors and Utilities_Information if a valid SPContext is available
    /// Falls back to old logging method other wise
    /// </summary>
    class SPListLogger : ILogUtility
    {
        ILogUtility logger = new LogUtility();
        IListOperations listOPs = new ListOperations();
        SPList Errors
        {
            get
            {
                return tryGetList("Utilities_Errors");
            }
        }

        private SPList tryGetList(string listTitle)
        {
            listTitle.RequireNotNullOrEmpty("listTitle");
            if (null == SPContext.Current || null == SPContext.Current.Web)
            {
                return null;
            }
            SPList listToRetrieve = null;
            SPWeb web = SPContext.Current.Web;
            string url = web.Url;
            SPSecurity.RunWithElevatedPrivileges( ()=>
            {
                using (SPSite site = new SPSite(url))
                using(SPWeb website = site.OpenWeb())
                {
                    website.AllowUnsafeUpdates = true;                    
                    listToRetrieve = website.Lists.TryGetList(listTitle);
                    if (null == listToRetrieve)
                    {
                        logger.TraceDebugInformation(string.Format("Creating Logging list at {0}", website.Url), GetType());
                        listToRetrieve = listOPs.CreateList(website, listTitle, "Logging List", SPListTemplateType.GenericList);
                        listToRetrieve.OnQuickLaunch = false;
                        listToRetrieve.Update();
                    }
                }

            });

            return listToRetrieve;
        }

        SPList Information
        {
            get
            {
                return tryGetList("Utilities_Information");
            }
        }
            
        public void TraceDebugException(string ErrorMessage, Type objType, Exception exception)
        {
            ErrorMessage.RequireNotNullOrEmpty("ErrorMessage");
            objType.RequireNotNull("objType");
            exception.RequireNotNull("exception");
            try
            {
                SPList list = Errors;
                if (null != list)
                {
                    WriteLongMessage(string.Format("Type: {0} {1}\r\nException: {2}", objType, ErrorMessage, exception), list);
                }
                else
                {
                    logger.TraceDebugException(ErrorMessage, objType, exception);
                }
            }
            catch (Exception internalException)
            {
                logger.TraceDebugException("SPListLogger internal exception", GetType(), internalException);
                logger.TraceDebugException(ErrorMessage, objType, exception);
            }
        }

        public void TraceDebugInformation(string MethodMessage, Type objType)
        {
            MethodMessage.RequireNotNullOrEmpty("MethodMessage");
            objType.RequireNotNull("objType");
            try
            {
                SPList list = Information;
                if (null != list)
                {
                    WriteLongMessage(string.Format("Type: {0} Message: {1}", objType, MethodMessage), list);
                }

                else
                {
                    logger.TraceDebugInformation(MethodMessage, objType);
                }
            }
            catch (Exception internalException)
            {
                logger.TraceDebugException("SPListLogger internal exception", GetType(), internalException);
                logger.TraceDebugInformation(MethodMessage, objType);
            }
        }

        /// <summary>
        /// Writes messages longer than 255 characters
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="list">The list.</param>
        private void WriteLongMessage(string message, SPList list)
        {
            message.RequireNotNullOrEmpty("message");
            list.RequireNotNull("list");
            if (message.Length <= 255)
            {
                WriteMessage(message, list);
            }

            else
            {
                for (int i = 0; i < message.Length; i += 255)
                {
                    int length = Math.Min(message.Length - i, 255);
                    WriteMessage(message.Substring(i, length), list);
                } 
            }
        }


        /// <summary>
        /// Writes messages of 255 characters or less
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="list">The list.</param>
        private void WriteMessage(string message, SPList list)
        {
            message.RequireNotNullOrEmpty("message");
            list.RequireNotNull("list");
            message.Require(message.Length <= 255, "message");
            SPListItem item = list.AddItem();
            item["Title"] = message;
            item.Update();
        }
    }
}
