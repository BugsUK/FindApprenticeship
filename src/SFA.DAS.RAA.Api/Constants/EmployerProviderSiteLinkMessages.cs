namespace SFA.DAS.RAA.Api.Constants
{
    using Apprenticeships.Web.Common.Constants;

    public class EmployerProviderSiteLinkMessages
    {
        public const string MissingEmployerIdentifier = "You must specify the employer's EDSURN.";
        public const string MissingProviderSiteIdentifier = "You must specify either the provider site's ID or EDSURN.";

        public const string EmployerNotFoundFormat = "No employer was found matching EDSURN {0}.";
        public const string ProviderSiteNotFoundIdFormat = "No provider site was found matching ID {0}.";
        public const string ProviderSiteNotFoundEdsUrnFormat = "No provider site was found matching EDSURN {0}.";

        public const string UnauthorizedProviderSiteAccess = "You do not have permission to add a link to an employer for the specified provider site.";

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