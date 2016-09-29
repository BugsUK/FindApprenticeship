namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class ProviderSiteViewModelMessages
    {
        public const string EdsUrnAlreadyExists = "The supplied EDSURN is already associated with a provider site";
        public const string ProviderSiteCreatedSuccessfully = "New provider site added successfully";
        public const string ProviderSiteSavedSuccessfully = "The changes to the provider site were saved successfully";
        public const string ProviderSiteSaveError = "An error occured when saving the provider site. Please check your entries and try again";
        public const string ProviderSiteRelationshipInvalidUkprn = "The supplied UKPRN does not map to a provider";
        public const string ProviderSiteRelationshipCreatedSuccessfully = "Successfully created a new provider site relationship";
        public const string ProviderSiteRelationshipCreationError = "An error occured when trying to save the new provider site relationship. Please check your entries and try again";

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
            public const string TooLongErrorText = "Full name must not be more than 255 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Full name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class TradingName
        {
            public const string LabelText = "Trading name";
            public const string RequiredErrorText = "Enter the provider sites's trading name";
            public const string TooLongErrorText = "Trading name must not be more than 255 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Trading name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class WebPage
        {
            public const string LabelText = "Web page";
            public const string RequiredErrorText = "Enter the provider sites's web site";
            public const string TooLongErrorText = "Web site must not be more than 100 characters";
            public const string ErrorUriText = "Enter a valid website url";
        }

        public static class ProviderUkprn
        {
            public const string LabelText = "Provider UKPRN";
            public const string RequiredErrorText = "Enter the provider's UKPRN";
            public const string RequiredLengthErrorText = "UKPRN must be 8 digits";
            public const string WhiteListRegularExpression = Whitelists.NumericalWhitelist.RegularExpression;
            public const string WhiteListErrorText = "UKPRN " + Whitelists.NumericalWhitelist.ErrorText;
        }

        public static class ProviderSiteRelationshipType
        {
            public const string LabelText = "Relationship Type";
        }
    }
}