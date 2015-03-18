namespace SFA.Apprenticeships.Web.Common.Framework
{
    public static class StringExtensions
    {
        public static bool IsValidReturnUrl(this string returnUrl)
        {
            return !string.IsNullOrEmpty(returnUrl) && (returnUrl.StartsWith("/") || returnUrl.ToLower().StartsWith("%2f"));
        }
    }
}
