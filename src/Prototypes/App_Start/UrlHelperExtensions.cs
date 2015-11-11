using System;
using System.Web.Mvc;

namespace Prototypes.App_Start
{
    // Hacked up copy of Web.Common.UrlHelperExtensions
    public static class UrlHelperExtensions
    {
        public static string CdnContent(this UrlHelper urlHelper, string contentName, string localContentPath)
        {
            const string separator = "/";

            return urlHelper.Content(string.Join(separator, localContentPath, contentName));
        }

    }
}