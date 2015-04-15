namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Web;

    public class HelpCookieProvider : IHelpCookieProvider
    {
        private const string CookieName = "NAS.Help";

        private enum CookieKeys
        {
            ShowSearchTour
        }

        public bool ShowSearchTour(HttpContextBase httpContext)
        {
            return Show(httpContext, CookieKeys.ShowSearchTour);
        }

        private static bool Show(HttpContextBase httpContext, CookieKeys cookeKey)
        {
            var httpCookie = httpContext.Request.Cookies.Get(CookieName);

            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(CookieName)
                {
                    Expires = DateTime.UtcNow.AddMonths(12)
                };
            }
            else
            {
                var cookieValue = httpCookie.Values[cookeKey.ToString()];
                if (!string.IsNullOrEmpty(cookieValue) && !bool.Parse(cookieValue))
                {
                    return false;
                }
            }

            httpCookie[cookeKey.ToString()] = false.ToString();

            httpContext.Response.Cookies.Add(httpCookie);

            return true;
        }
    }
}