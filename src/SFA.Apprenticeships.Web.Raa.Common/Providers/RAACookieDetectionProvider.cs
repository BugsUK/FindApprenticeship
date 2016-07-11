namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Web;
    using Web.Common.Providers;

    public class RaaCookieDetectionProvider : ICookieDetectionProvider
    {
        private const string CookieDetection = "RAA.CookieDetection";

        public void SetCookie(HttpContextBase httpContext)
        {
            var httpCookie = httpContext.Request.Cookies.Get(CookieDetection);

            if (httpCookie != null)
            {
                return;
            }

            var cookie = new HttpCookie(CookieDetection)
            {
                Expires = DateTime.UtcNow.AddMonths(12)
            };
            httpContext.Response.Cookies.Add(cookie);
        }

        public bool IsCookiePresent(HttpContextBase httpContext)
        {
            var httpCookie = httpContext.Request.Cookies.Get(CookieDetection);
            return httpCookie != null;
        }
    }
}