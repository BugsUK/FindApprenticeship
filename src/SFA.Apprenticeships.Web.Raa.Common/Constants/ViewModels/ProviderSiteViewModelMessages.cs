namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class ProviderSiteViewModelMessages
    {
        public const string EdsUrnAlreadyExists = "The supplied EDSURN is already associated with a provider site";
        public const string ProviderSiteCreatedSuccessfully = "New provider site added successfully";

        public static class DisplayName
        {
            public const string LabelText = "Provider site";
        }

        public static class EdsUrn
        {
            public const string LabelText = "EDSURN";
            public const string RequiredErrorText = "Enter the provider site's EDSURN";
            public const string RequiredLengthErrorText = "EDSURN must be 9 digits";
            public const string WhiteListRegularExpression = Whitelists.NumericalWhitelist.RegularExpression;
            public const string WhiteListErrorText = "EDSURN " + Whitelists.NumericalWhitelist.ErrorText;
        }

        public static class FullName
        {
            public const string LabelText = "Full name";
            public const string RequiredErrorText = "Enter the provider site's full name";
            public const string TooLongErrorText = "Full name must not be more than 100 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Full name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class TradingName
        {
            public const string LabelText = "Trading name";
            public const string RequiredErrorText = "Enter the provider sites's Trading name";
            public const string TooLongErrorText = "Trading name must not be more than 100 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Trading name " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}