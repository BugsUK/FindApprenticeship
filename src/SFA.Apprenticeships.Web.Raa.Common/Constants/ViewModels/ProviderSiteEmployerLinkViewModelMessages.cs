namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class ProviderSiteEmployerLinkViewModelMessages
    {
        public static class Description
        {
            public const string LabelText = "Employer description for candidates";
            public const string RequiredErrorText = "Enter an employer description for candidates";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Employer description for candidates " + Whitelists.FreetextWhitelist.ErrorText;
        }
        public static class WebsiteUrl
        {
            public const string LabelText = "Website (optional)";
            public const string ErrorUriText = "Enter a valid website url";
        }

        public class NumberOfPositions
        {
            public const string LabelText = "Number of positions for this vacancy";
            public const string RequiredErrorText = "Enter the number of positions for this vacancy";
            public const string LengthErrorText = "You must enter at least 1 position";
        }

        public class IsEmployerLocationMainApprenticeshipLocation
        {
            public const string RequiredErrorText = "Select whether the employer’s address is the vacancy location or not";
        }

        public class DescriptionComment
        {
            public const string LabelText = "Employer description for candidates comment";
        }

        public class WebsiteUrlComment
        {
            public const string LabelText = "Website comment";
        }

        public class NumberOfPositionsComment
        {
            public const string LabelText = "Number of positions for this vacancy comment";
        }
    }
}