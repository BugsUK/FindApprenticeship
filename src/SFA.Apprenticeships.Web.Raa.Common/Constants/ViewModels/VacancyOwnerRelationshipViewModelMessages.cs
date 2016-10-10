namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class VacancyOwnerRelationshipViewModelMessages
    {
        public static class EmployerDescription
        {
            public const string LabelText = "About the employer";
            public const string RequiredErrorText = "Enter text about the employer";
            public const string WhiteListHtmlRegularExpression = Whitelists.FreeHtmlTextWhiteList.RegularExpression;
            public const string WhiteListTextRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListInvalidCharacterErrorText = "About the employer " + Whitelists.FreeHtmlTextWhiteList.InvalidCharacterErrorText;
            public const string WhiteListInvalidTagErrorText = "About the employer " + Whitelists.FreeHtmlTextWhiteList.InvalidTagErrorText;
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

        public class InvalidEmployerAddress
        {
            public const string ErrorText = "We could not map the address. You will need to complete the full address to continue.";
        }
    }
}