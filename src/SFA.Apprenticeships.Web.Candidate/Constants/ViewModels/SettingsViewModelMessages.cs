﻿namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class SettingsViewModelMessages
    {
        public static class FirstnameMessages
        {
            public const string LabelText = "First name";
            public const string RequiredErrorText = "Please enter first name";
            public const string TooLongErrorText = "First name mustn’t exceed {0} characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "First name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class LastnameMessages
        {
            public const string LabelText = "Last name";
            public const string RequiredErrorText = "Please enter last name";
            public const string TooLongErrorText = "Last name mustn’t exceed {0} characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Last name " + Whitelists.FreetextWhitelist.ErrorText;
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

        public static class SubmitApplicationForm
        {
            public const string LabelText = "you submit an application form";
        }

        public static class ApplicationStatusChange
        {
            public const string LabelText = "the status of one of your applications changes";
        }

        public static class ApplicationExpiring
        {
            public const string LabelText = "an apprenticeship is approaching its closing date";
        }

        public static class MarketingComms
        {
            public const string LabelText = "we send you updates on news and information";
        }

        public static class SavedSearch
        {
            public const string SendSavedSearchAlertsViaEmailLabelText = "Email";
            public const string SendSavedSearchAlertsViaTextLabelText = "Text";
            public const string AlertsEnabledLabelText = "Receive alerts for this search";
        }
    }
}
