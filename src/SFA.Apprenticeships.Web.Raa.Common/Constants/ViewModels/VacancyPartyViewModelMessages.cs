namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class VacancyPartyViewModelMessages
    {
        public static class EmployerDescription
        {
            public const string LabelText = "About the employer";
            public const string RequiredErrorText = "Enter text about the employer";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "About the employer " + Whitelists.FreetextWhitelist.ErrorText;
        }
        public static class EmployerWebsiteUrl
        {
            public const string LabelText = "Employer website (optional)";
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

        public class EmployerDescriptionComment
        {
            public const string LabelText = "Employer description for candidates comment";
        }

        public class EmployerWebsiteUrlComment
        {
            public const string LabelText = "Website comment";
        }

        public class NumberOfPositionsComment
        {
            public const string LabelText = "Number of positions for this vacancy comment";
        }
    }
}