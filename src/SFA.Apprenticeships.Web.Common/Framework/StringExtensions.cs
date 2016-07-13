namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;

    public static class StringExtensions
    {
        public static bool IsValidReturnUrl(this string returnUrl)
        {
            return !string.IsNullOrEmpty(returnUrl) && (returnUrl.StartsWith("/") || returnUrl.ToLower().StartsWith("%2f")) && returnUrl != "/" && returnUrl != "%2f";
        }

        public static bool IsValidReturnUrlIncludingRoot(this string returnUrl)
        {
            return !string.IsNullOrEmpty(returnUrl) && (returnUrl.StartsWith("/") || returnUrl.ToLower().StartsWith("%2f"));
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
    }
}
