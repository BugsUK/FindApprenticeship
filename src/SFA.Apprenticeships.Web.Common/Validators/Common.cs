namespace SFA.Apprenticeships.Web.Common.Validators
{
    using System;
    using System.Text.RegularExpressions;
    using Constants;
    using ViewModels;

    public class Common
    {
        public static bool BeValidDate(DateViewModel instance)
        {
            return BeValidDate(instance, null);
        }

        public static bool BeValidDate(DateViewModel instance, int? day)
        {
            if (instance == null) return false;
            try
            {
                var date = instance.Date;
                return date != DateTime.MinValue;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public static bool BeTwoWeeksInTheFuture(DateViewModel instance)
        {
            return BeInTheFuture(instance, 14);
        }

        public static bool BeOneDayInTheFuture(DateViewModel instance)
        {
            return BeInTheFuture(instance, 1);
        }

        public static bool BeTodayOrInTheFuture(DateViewModel instance)
        {
            return BeInTheFuture(instance, 0);
        }

        private static bool BeInTheFuture(DateViewModel instance, int daysInFuture)
        {
            //We don't have a value for date yet so assume it's in the future
            if (instance == null || !instance.HasValue)
            {
                return true;
            }

            //It's not a valid date yet so again assume true
            if (!BeValidDate(instance))
            {
                return true;
            }

            return instance.Date >= DateTime.Today.AddDays(daysInFuture);
        }

        private static readonly Regex ProtocolRegex = new Regex("^(.+?)://");

        public static bool IsValidUrl(string uri)
        {
            if (string.IsNullOrEmpty(uri)) { return false; }
            if (!Regex.Replace(uri, "www\\.", "", RegexOptions.IgnoreCase).Contains(".")) { return false; }
            var uriDomain = ProtocolRegex.Replace(uri, "");
            if (uriDomain.Contains("/"))
            {
                uriDomain = uriDomain.Substring(0, uriDomain.IndexOf("/", StringComparison.Ordinal));
            }
            uriDomain = uri.Substring(0, uri.IndexOf(uriDomain, StringComparison.Ordinal) + uriDomain.Length);
            if (uriDomain.Split(' ').Length > 1) return false;
            if (ProtocolRegex.IsMatch(uri))
            {
                var protocol = ProtocolRegex.Match(uri).Groups[1].Value;
                if (!protocol.StartsWith("http")) { return false; }
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

        public static bool IsNotEmpty(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
    }
}