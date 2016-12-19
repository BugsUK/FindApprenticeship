namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class SettingsViewModelMessages
    {
        public static class FirstnameMessages
        {
            public const string LabelText = "First name";
            public const string RequiredErrorText = "Please enter first name";
            public const string TooLongErrorText = "First name must not be more than {0} characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "First name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class LastnameMessages
        {
            public const string LabelText = "Last name";
            public const string RequiredErrorText = "Please enter last name";
            public const string TooLongErrorText = "Last name must not be more than {0} characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Last name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class EmailAddressMessages
        {
            public const string LabelText = "Email address";
            public const string RequiredErrorText = "Please enter email address";
            public const string InvalidErrorText = "Please enter a valid email address and password";
        }

        public static class PasswordMessages
        {
            public const string LabelText = "Password";
            public const string RequiredErrorText = "Please enter password";
            public const string InvalidErrorText = "Please enter a valid email address and password";
        }

        public static class DateOfBirthMessages
        {
            public const string LabelText = "Date of birth";
        }

        public static class PhoneNumberMessages
        {
            public const string LabelText = "Phone";
            public const string RequiredErrorText = "Please enter phone number";
            public const string LengthErrorText = "Phone number must be between {0} and {1} digits";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Phone number " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }

        public static class AllowEmailMessages
        {
            public const string LabelText = "Email";
        }

        public static class AllowSmsMessages
        {
            public const string LabelText = "Text";
        }

        public static class ApplicationStatusChanges
        {
            public const string LabelText = "the status of one of your applications changes";
        }

        public static class ExpiringApplications
        {
            public const string LabelText = "an apprenticeship is approaching its closing date";
        }

        public static class Marketing
        {
            public const string LabelText = "we send you updates on news and information";
        }

        public static class SavedSearch
        {
            public const string EmailLabelText = "Email";
            public const string TextLabelText = "Text";
            public const string AlertsEnabledLabelText = "Receive alerts for this search";
        }
    }
}
