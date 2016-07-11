namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Web;
    using Web.Common.Providers;

    public class RaaEuCookieDirectiveProvider : IEuCookieDirectiveProvider
    {
        private const string EuCookieName = "RAA.DisplayEuCookieDirective";

        public bool ShowEuCookieDirective(HttpContextBase httpContext)
        {
            var httpCookie = httpContext.Request.Cookies.Get(EuCookieName);

            if (httpCookie != null)
            {
                return false;
            }

            var cookie = new HttpCookie(EuCookieName)
            {
                Expires = DateTime.UtcNow.AddMonths(12)
            };

            httpContext.Response.Cookies.Add(cookie);

            return true;
        }
    }
}