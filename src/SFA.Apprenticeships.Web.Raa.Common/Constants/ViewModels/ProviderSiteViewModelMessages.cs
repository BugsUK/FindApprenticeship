namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class ProviderSiteViewModelMessages
    {
        public static class NameMessages
        {
            public const string LabelText = "Training site name";
            public const string RequiredErrorText = "Enter your training site name";
            public const string TooLongErrorText = "Training site name must not be more than 100 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Training site name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class EmailAddressMessages
        {
            public const string LabelText = "Key contact email address";
            public const string RequiredErrorText = "Enter the key contact email address";
            public const string TooLongErrorText = "Key contact email address must not be more than 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Key contact email address " + Whitelists.EmailAddressWhitelist.ErrorText;
        }

        public static class PhoneNumberMessages
        {
            public const string LabelText = "Phone number number";
            public const string RequiredErrorText = "Enter phone number";
            public const string LengthErrorText = "Phone number number must be between 8 and 16 digits";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Phone number " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }
    }
}