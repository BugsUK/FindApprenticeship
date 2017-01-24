﻿namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class RegisterViewModelMessages
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
            public const string LabelText = "Enter email address";
            public const string HintText = "You'll need this to sign in to your account. The email address you choose will be seen by employers.";
            public const string RequiredErrorText = "Please enter email address";
            public const string TooLongErrorText = "Email address must not be more than 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Email address " + Whitelists.EmailAddressWhitelist.ErrorText;
            public static string UsernameNotAvailableErrorText = "Your email address has already been activated. Please try signing in again. If you’ve forgotten your password you can reset it.";
        }

        public static class PhoneNumberMessages
        {
            public const string LabelText = "Enter mobile phone number";
            public const string HintText = "If you don't have a mobile, enter a landline number.";
            public const string RequiredErrorText = "Please enter phone number";
            public const string LengthErrorText = "Phone number must be between 8 and 16 digits";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Phone number " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }

        public static class PasswordMessages
        {
            public const string LabelText = "Create password";
            public const string HintText = "Requires upper and lowercase letters, a number and at least 8 characters";
            public const string RequiredErrorText = "Please enter password";
            public const string LengthErrorText = "Password must be at least 8 characters long, contain upper and lowercase letters and one number";
            public const string WhiteListRegularExpression = Whitelists.PasswordWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Password " + Whitelists.PasswordWhitelist.ErrorText;
            public const string PasswordsDoNotMatchErrorText = "Sorry, your passwords don’t match";
        }

        public static class AcceptUpdates
        {
            public const string LabelText = "I'd like to receive the latest careers news and updates";
        }

        public static class TermsAndConditions
        {
            public const string LabelText = "<span id=\"HasAcceptedTermsAndConditionsText\">I accept the </span><a href='/terms' target='_blank' onclick=\"Webtrends.multiTrack({ element: this, argsa: ['DCS.dcsuri', '/register/readterms', 'WT.dl', '99', 'WT.ti', 'Read Terms and Conditions'] });\">terms and conditions</a>";
            public const string MustAcceptTermsAndConditions = "Please accept the terms and conditions";
        }

        public class ConfirmPasswordMessages
        {
            public const string LabelText = "Confirm password";
            public const string RequiredErrorText = "Please confirm password";            
        }
    }
}
