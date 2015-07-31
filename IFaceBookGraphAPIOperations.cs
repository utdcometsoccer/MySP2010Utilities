using System;
using System.Collections;
using Microsoft.IdentityModel.Claims;

namespace MySP2010Utilities
{
    public interface IFaceBookGraphAPIOperations
    {
        Hashtable callGraphApi(Uri uri, string accessToken);
        string GetAccessToken(IClaimsIdentity identity);
    }
}
