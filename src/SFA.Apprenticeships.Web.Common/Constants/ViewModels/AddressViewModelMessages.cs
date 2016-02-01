namespace SFA.Apprenticeships.Web.Common.Constants.ViewModels
{
    using Constants;

    public static class AddressViewModelMessages
    {
        public static class AddressLine1
        {
            public const string LabelText = "Address";
            public const string RequiredErrorText = "Enter your first line of address";
            public const string TooLongErrorText = "First line of address must not be more than {0} characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "First line of address " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class AddressLine2
        {
            public const string LabelText = "Second line (optional)";
            public const string TooLongErrorText = "Second line of address must not be more than {0} characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Second line of address " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class AddressLine3
        {
            public const string LabelText = "Third line (optional)";
            public const string TooLongErrorText = "Third line of address must not be more than {0} characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Third line of address " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class AddressLine4
        {
            public const string LabelText = "Fourth line (optional)";
            public const string TooLongErrorText = "Fourth line of address must not be more than {0} characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Fourth line of address " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class AddressLine5
        {
            public const string LabelText = "Fifth line (optional)";
            public const string TooLongErrorText = "Fifth line of address must not be more than {0} characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Fifth line of address " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class Postcode
        {
            public const string LabelText = "Postcode";
            public const string RequiredErrorText = "Enter your postcode";
            public const string TooLongErrorText = "Postcode must not be more than 8 characters";
            public const string WhiteListRegularExpression = Whitelists.PostcodeWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Postcode' " + Whitelists.PostcodeWhitelist.ErrorText;
        }
    }
}