using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using SFA.Apprenticeships.Web.Common.Models.Common;

namespace SFA.Apprenticeships.Web.Common.Providers
{
    public class CookieAuthorizationDataProvider : ICookieAuthorizationDataProvider
    {
        private const string CookieName = "User.Authorization";
        private const string Purpose = "Authorization Claims";

        public void AddClaim(Claim claim, HttpContextBase httpContext, string username)
        {
            var authorizationData = GetAuthorizationData(httpContext, username);

            var claims = authorizationData.Claims.ToList();
            var authorizationClaim = new AuthorizationClaim {Type = claim.Type, Value = claim.Value};
            claims.Add(authorizationClaim);
            authorizationData.Claims = claims.ToArray();

            var authorizationDataJson = JsonConvert.SerializeObject(authorizationData);
            var protectedAuthorizationData = Protect(authorizationDataJson);

            var cookie = new HttpCookie(CookieName, protectedAuthorizationData);
            
            //TODO: http + secure
            if (httpContext.Response.Cookies.AllKeys.Contains(CookieName))
            {
                httpContext.Response.Cookies.Set(cookie);
            }
            else
            {
                httpContext.Response.Cookies.Add(cookie);
            }
        }

        public Claim[] GetClaims(HttpContextBase httpContext, string username)
        {
            //TODO: Check ukprn and username
            var authorizationData = GetAuthorizationData(httpContext, username);
            var claims = authorizationData.Claims.Select(c => new Claim(c.Type, c.Value)).ToArray();
            return claims;
        }

        public void Clear(HttpContextBase httpContext)
        {
            if (!httpContext.Request.Cookies.AllKeys.Contains(CookieName))
            {
                return;
            }

            httpContext.Request.Cookies.Remove(CookieName);
        }

        private static AuthorizationData GetAuthorizationData(HttpContextBase httpContext, string username)
        {
            //TODO: Check ukprn and username
            var cookie = GetCookie(httpContext);
            if (string.IsNullOrEmpty(cookie?.Value))
            {
                return new AuthorizationData {Username = username};
            }
            var authorizationDataJson = Unprotect(cookie.Value);
            var authorizationData = JsonConvert.DeserializeObject<AuthorizationData>(authorizationDataJson);
            return authorizationData;
        }

        private static HttpCookie GetCookie(HttpContextBase httpContext)
        {
            if (httpContext.Response.Cookies.AllKeys.Contains(CookieName))
            {
                return httpContext.Response.Cookies.Get(CookieName);
            }
            if (httpContext.Request.Cookies.AllKeys.Contains(CookieName))
            {
                return httpContext.Request.Cookies.Get(CookieName);
            }
            return null;
        }

        //http://stackoverflow.com/questions/3681493/leveraging-asp-net-machinekey-for-encrypting-my-own-data
        public static string Protect(string unprotectedText)
        {
            var unprotectedBytes = Encoding.UTF8.GetBytes(unprotectedText);
            var protectedBytes = MachineKey.Protect(unprotectedBytes, Purpose);
            var protectedText = Convert.ToBase64String(protectedBytes);
            return protectedText;
        }

        public static string Unprotect(string protectedText)
        {
            var protectedBytes = Convert.FromBase64String(protectedText);
            var unprotectedBytes = MachineKey.Unprotect(protectedBytes, Purpose);
            var unprotectedText = Encoding.UTF8.GetString(unprotectedBytes);
            return unprotectedText;
        }
    }
}