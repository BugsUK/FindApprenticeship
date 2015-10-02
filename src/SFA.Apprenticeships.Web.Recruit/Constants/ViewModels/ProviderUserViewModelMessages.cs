namespace SFA.Apprenticeships.Web.Recruit.Constants.ViewModels
{
    using Common.Constants;

    public class ProviderUserViewModelMessages
    {
        public const string AccountCreated = "You've successfully created your account";
        public const string EmailUpdated = "Your email address has been changed successfully";
        public const string AccountUpdated = "Your details have been updated successfully";

        public static class FullnameMessages
        {
            public const string LabelText = "Full name";
            public const string RequiredErrorText = "Please enter your Full name";
            public const string TooLongErrorText = "Full name mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Full name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class EmailAddressMessages
        {
            public const string LabelText = "Work email address";
            public const string RequiredErrorText = "Please enter work email address";
            public const string TooLongErrorText = "Work email address mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Work email address " + Whitelists.EmailAddressWhitelist.ErrorText;
        }

        public static class PhoneNumberMessages
        {
            public const string LabelText = "Work phone number";
            public const string RequiredErrorText = "Please enter work phone number";
            public const string LengthErrorText = "Work phone number must be between 8 and 16 digits";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Work phone number " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }
    }
}