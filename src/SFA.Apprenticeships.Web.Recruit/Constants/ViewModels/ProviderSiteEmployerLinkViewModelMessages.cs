namespace SFA.Apprenticeships.Web.Recruit.Constants.ViewModels
{
    using Common.Constants;

    public class ProviderSiteEmployerLinkViewModelMessages
    {
        public static class Description
        {
            public const string LabelText = "Employer description for your vacancies";
            public const string RequiredErrorText = "Please provide an employer description for your vacancies";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Employer description for your vacancies " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}