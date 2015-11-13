namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;

    public static class UrlHelperExtensions
    {
        private const string separator = "/";

        [Obsolete("Use bundling for scripts and css. Use CdnImage for images.")]

        public static string CdnContent(this UrlHelper urlHelper, string relativeContentName, string localContentPath)
        {
            return NonObsoleteCdnContent(urlHelper, relativeContentName, localContentPath);
        }

        public static string CdnImage(this UrlHelper urlHelper, string contentNameRelativeToImg)
        {
            return NonObsoleteCdnContent(urlHelper, "img" + separator + contentNameRelativeToImg, "~/Content/_assets");
        }

        private static string NonObsoleteCdnContent(UrlHelper urlHelper, string contentNameWithRelativePath, string localContentPath)
        {
            return IsLocalEnvironment ?
                urlHelper.Content(string.Join(separator, localContentPath, contentNameWithRelativePath)) :
                string.Join(separator, CdnUrl, EnvironmentName, contentNameWithRelativePath).ToLower();
        }
        #region Helpers

        private static string EnvironmentName
        {
            get
            {
                return ConfigurationManager.AppSettings["Environment"];
            }
        }

        private static string CdnUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["CdnUrl"];
            }
        }

        private static bool IsLocalEnvironment
        {
            get
            {
                var localEnvironmentNames = new [] { "debug", "local", "dev", "demo", "sit" };

                return
                    localEnvironmentNames.Any(each =>
                        each.Equals(EnvironmentName, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        #endregion
    }
}
