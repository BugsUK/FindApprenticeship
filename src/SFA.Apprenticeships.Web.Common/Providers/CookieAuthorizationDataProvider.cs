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
    using SFA.Infrastructure.Interfaces;

    public class CookieAuthorizationDataProvider : ICookieAuthorizationDataProvider
    {
        private const string CookieName = "User.Authorization";
        private const string Purpose = "Authorization Claims";

        private readonly ILogService _logService;

        public CookieAuthorizationDataProvider(ILogService logService)
        {
            _logService = logService;
        }

        public void AddClaim(Claim claim, HttpContextBase httpContext, string username)
        {
            var authorizationData = GetAuthorizationData(httpContext, username);

            var claims = authorizationData.Claims.ToList();
            var authorizationClaim = new AuthorizationClaim {Type = claim.Type, Value = claim.Value};
            claims.Add(authorizationClaim);
            authorizationData.Claims = claims.ToArray();
            SetCookie(httpContext, authorizationData);
        }

        public void RemoveClaim(string claimType, string claimValue, HttpContextBase httpContext, string username)
        {
            var authorizationData = GetAuthorizationData(httpContext, username);
            var claimToRemove = authorizationData.Claims.SingleOrDefault(c => c.Type == claimType && c.Value == claimValue);

            if (claimToRemove != null)
            {
                authorizationData.Claims = authorizationData.Claims.Where(c => c != claimToRemove).ToArray();
                SetCookie(httpContext, authorizationData);
            }
        }

        public Claim[] GetClaims(HttpContextBase httpContext, string username)
        {
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

            var cookie = new HttpCookie(CookieName) {Expires = DateTime.Now.AddDays(-1d)};
            if (httpContext.Response.Cookies.AllKeys.Contains(CookieName))
            {
                httpContext.Response.Cookies.Set(cookie);
            }
            else
            {
                httpContext.Response.Cookies.Add(cookie);
            }
        }

        private AuthorizationData GetAuthorizationData(HttpContextBase httpContext, string username)
        {
            var cookie = GetCookie(httpContext);
            if (string.IsNullOrEmpty(cookie?.Value))
            {
                return new AuthorizationData {Username = username};
            }
            try
            {
                var authorizationDataJson = Unprotect(cookie.Value);
                var authorizationData = JsonConvert.DeserializeObject<AuthorizationData>(authorizationDataJson);
                if (authorizationData.Username != username)
                {
                    Clear(httpContext);
                    authorizationData = new AuthorizationData { Username = username };
                }
                return authorizationData;
            }
            catch (Exception ex)
            {
                _logService.Warn("Unprotecting or deserializing " + CookieName + " failed.", ex);
                Clear(httpContext);
                return new AuthorizationData { Username = username };
            }
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

        private static void SetCookie(HttpContextBase httpContext, AuthorizationData authorizationData)
        {
            var authorizationDataJson = JsonConvert.SerializeObject(authorizationData);
            var protectedAuthorizationData = Protect(authorizationDataJson);

            var cookie = new HttpCookie(CookieName, protectedAuthorizationData);

            if (httpContext.Response.Cookies.AllKeys.Contains(CookieName))
            {
                httpContext.Response.Cookies.Set(cookie);
            }
            else
            {
                httpContext.Response.Cookies.Add(cookie);
            }
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
            if (unprotectedBytes != null)
            {
                var unprotectedText = Encoding.UTF8.GetString(unprotectedBytes);
                return unprotectedText;
            }
            return "";
        }
    }
}