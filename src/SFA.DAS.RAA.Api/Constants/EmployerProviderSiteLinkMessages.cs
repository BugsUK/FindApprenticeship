namespace SFA.DAS.RAA.Api.Constants
{
    using Apprenticeships.Web.Common.Constants;

    public class EmployerProviderSiteLinkMessages
    {
        public const string MissingEmployerIdentifier = "You must specify either the employer's ID or EDSURN.";
        public const string MissingProviderSiteIdentifier = "You must specify either the provider site's ID or EDSURN.";

        public static class EmployerDescription
        {
            public const string RequiredErrorText = "Please supply a description for the employer.";
            public const string WhiteListHtmlRegularExpression = Whitelists.FreeHtmlTextWhiteList.RegularExpression;
            public const string WhiteListInvalidCharacterErrorText = "The description for the employer " + Whitelists.FreeHtmlTextWhiteList.InvalidCharacterErrorText + ".";
            public const string WhiteListInvalidTagErrorText = "The description for the employer " + Whitelists.FreeHtmlTextWhiteList.InvalidTagErrorText + ".";
        }
        public static class EmployerWebsiteUrl
        {
            public const string InvalidUrlText = "Please supply a valid website url for the employer.";
        }
    }
}