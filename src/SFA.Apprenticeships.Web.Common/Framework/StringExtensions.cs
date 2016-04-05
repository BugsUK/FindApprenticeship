namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static bool IsValidReturnUrl(this string returnUrl)
        {
            return !string.IsNullOrEmpty(returnUrl) && (returnUrl.StartsWith("/") || returnUrl.ToLower().StartsWith("%2f")) && returnUrl != "/" && returnUrl != "%2f";
        }

        public static T? GetValueOrNull<T>(this string valueAsString) where T : struct
        {
            if (string.IsNullOrEmpty(valueAsString)) return null;
            return (T)Convert.ChangeType(valueAsString, typeof(T));
        }

        public static T GetValueOrDefault<T>(this string valueAsString, T defaultValue = default(T)) where T : struct
        {
            if (string.IsNullOrEmpty(valueAsString)) return defaultValue;
            return (T)Convert.ChangeType(valueAsString, typeof(T));
        }

        public static string RemoveHtmlTag(this string valueAsString)
        {
            const string HTML_TAG_PATTERN = "<.*?>";
            StringBuilder sb = new StringBuilder(valueAsString);
            sb.Replace("& bull;", string.Empty);
            sb.Replace("&nbsp;", string.Empty);
            return Regex.Replace(sb.ToString(), HTML_TAG_PATTERN, string.Empty);
        }

    }
}
