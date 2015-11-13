namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class ProviderSiteEmployerLinkViewModelMessages
    {
        public static class Description
        {
            public const string LabelText = "Employer description for your vacancies";
            public const string RequiredErrorText = "Please enter an employer description for your vacancies";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Employer description for your vacancies " + Whitelists.FreetextWhitelist.ErrorText;
        }
        public static class WebsiteUrl
        {
            public const string LabelText = "Website (optional)";
            public const string ErrorUriText = "Please enter a valid website url";
        }
    }
}