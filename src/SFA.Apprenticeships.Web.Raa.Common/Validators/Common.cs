namespace SFA.Apprenticeships.Web.Raa.Common.Validators
{
    using System;
    using System.Text.RegularExpressions;
    using Web.Common.Constants;

    public static class Common
    {
        private static readonly Regex ProtocolRegex = new Regex("^(.+?)://");

        public static bool IsValidUrl(string uri)
        {
            if (string.IsNullOrEmpty(uri)) { return false; }
            if (!Regex.Replace(uri, "www\\.", "", RegexOptions.IgnoreCase).Contains(".")) { return false; }
            if (uri.Split(' ').Length > 1) return false;
            if (ProtocolRegex.IsMatch(uri))
            {
                var protocol = ProtocolRegex.Match(uri).Groups[1].Value;
                if(!protocol.StartsWith("http")) { return false; }
            }
            if (!uri.StartsWith("http://") && !uri.StartsWith("https://")) { uri = $"http://{uri}"; }
            try
            {
                Uri outUri;
                if (Uri.TryCreate(uri, UriKind.Absolute, out outUri)
                    && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool BeAValidFreeText(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                //Will be picked up by required validator
                return true;
            }
            if (Regex.IsMatch(input, Whitelists.FreeHtmlTextWhiteList.RegularExpressionScripts) ||
                Regex.IsMatch(input, Whitelists.FreeHtmlTextWhiteList.RegularExpressionInputs) ||
                Regex.IsMatch(input, Whitelists.FreeHtmlTextWhiteList.RegularExpressionObjects))
            {
                return false;
            }
            return true;
        }
    }
}