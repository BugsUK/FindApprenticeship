namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class LocationSearchViewModelMessages
    {
        public static class AdditionalLocationInformation
        {
            public const string LabelText = "Additional location information (optional)";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "The title " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public class PostCodeSearch
        {
            public const string LabelText = "Enter the employer's postcode";
        }
    }
}