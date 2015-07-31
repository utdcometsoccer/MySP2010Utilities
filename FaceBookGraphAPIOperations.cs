using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using Microsoft.IdentityModel.Claims;

namespace MySP2010Utilities
{
    class FaceBookGraphAPIOperations : IFaceBookGraphAPIOperations
    {
        public Hashtable callGraphApi(Uri uri, string accessToken)
        {
            UriBuilder builder = new UriBuilder(uri);
            if (!string.IsNullOrEmpty(builder.Query))
            {
                builder.Query += "&";
            }
            builder.Query += "access_token=" + accessToken;
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

            using (WebClient client = new WebClient())
            {
                string data = client.DownloadString(builder.ToString());
                return jsSerializer.Deserialize<Hashtable>(data);
            }
        }

        public string GetAccessToken(IClaimsIdentity identity)
        {
            var accessToken = (from claim in identity.Claims
                               where claim.ClaimType == "http://www.facebook.com/claims/AccessToken"
                               select (string)claim.Value).FirstOrDefault();
            return accessToken;
        }
    }
}
