using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.Office.Server.UserProfiles;

namespace MySP2010Utilities
{
    class UserParser : MySP2010Utilities.IUserParser
    {
        public string parseUsers(string userValue, char separator, SPSite site)
        {
            string[] userData = userValue.Split(separator);
            if (userData.Count() == 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder(parseUser(userData[0], site));
            foreach (string userString in userData.Skip(1))
            {
                sb.AppendFormat("{0}{1}", separator, parseUser(userString, site));
            }
            return sb.ToString();
        }

        public string parseUser(string userValue, SPSite site)
        {
            try
            {
                SPServiceContext svcCtx = SPServiceContext.GetContext(site);
                UserProfileManager profileManager = new UserProfileManager(svcCtx);
                var profile = profileManager.GetUserProfile(userValue);
                return null != profile["WorkEmail"].Value ? profile["WorkEmail"].ToString() : string.Empty;
            }
            catch (Exception exception)
            {
                SharePointUtilities.TraceDebugException(" Could not parse user! ", GetType(), exception);
                return string.Empty;
            }
        }
    }
}
