namespace SFA.Apprenticeships.Web.ContactForms.Constants
{
    public class AccessRequestViewModelMessages
    {
        public static class FirstnameMessages
        {
            public const string LabelText = "First name";
            public const string RequiredErrorText = "Please enter first name";
            public const string TooLongErrorText = "First name mustn’t exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "First name " + Whitelists.NameWhitelist.ErrorText;
        }

        public static class LastnameMessages
        {
            public const string LabelText = "Last name";
            public const string RequiredErrorText = "Please enter last name";
            public const string TooLongErrorText = "Last name mustn’t exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Last name " + Whitelists.NameWhitelist.ErrorText;
        }

        public static class CompanynameMessages
        {
            public const string LabelText = "Company name";
            public const string RequiredErrorText = "Please enter your company name";
            public const string TooLongErrorText = "Company name mustn’t exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Company name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class PositionMessages
        {
            public const string LabelText = "Position at company";
            public const string RequiredErrorText = "Please enter position at company";
            public const string TooLongErrorText = "Position mustn’t exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Position " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class EmailAddressMessages
        {
            public const string LabelText = "Email address";

            public const string HintText =
                "This'll be used for further communication to resolve your query. Please make sure you enter an valid email address";

            public const string RequiredErrorText = "Please enter email address";
            public const string TooLongErrorText = "Email address mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Email address " + Whitelists.EmailAddressWhitelist.ErrorText;
        }

        public static class ConfirmEmailAddressMessages
        {
            public const string LabelText = "Re-Confirm email address";
            public const string RequiredErrorText = "Please re-confirm email address";
            public const string TooLongErrorText = "Confirm email address mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Confirm email address " + Whitelists.EmailAddressWhitelist.ErrorText;
            public const string ConfirmEmailNotMatchingErrorText = "Confirm email does not match with email address";
            
        }

        public static class WorkPhoneNumberMessages
        {
            public const string LabelText = "Work phone number";
            public const string HintText = "If you don't have a landline number, enter your mobile number";
            public const string RequiredErrorText = "Please enter work phone number";
            public const string LengthErrorText = "Work Phone number must be between 8 and 16 digits";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Work Phone number " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }

        public static class MobileNumberMessages
        {
            public const string LabelText = "Mobile number";
            public const string LengthErrorText = "Mobile number must be between 8 and 16 digits";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Mobile number " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }
        public static class HasVacanciesAdvertisedMessages
        {
            public const string LabelText = "Have you already got vacancies advertised on apprenticeships vacancies?";
        }

        public static class AccessRequestServicesMessages
        {
            public const string LabelText = "Services you require access?";
            public const string RequiredErrorText = "Please select services you require access";
        }
        
        public static class NameTitleMessages
        {
            public const string LabelText = "Title";
            public const string HintText = "Please select your name title";
            public const string RequiredErrorText = "Please select your name title";
        }

        public static class UserTypeMessages
        {
            public const string LabelText = "User type";
            public const string HintText = "Please select user type";
            public const string RequiredErrorText = "Please select user type";
        }

        public static class ContactnameMessages
        {
            public const string LabelText = "Contact name";
            public const string HintText = "If different to above";
        }

        public static class AdditionalMobileNumberMessages
        {
            public const string LabelText = "Contact phone number";
            public const string HintText = "If different to above";
            public const string LengthErrorText = "Contact phone number must be between 8 and 16 digits";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Contact phone number " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }

        public static class AdditionalEmailEmailMessages
        {
            public const string LabelText = "Email address";
            public const string HintText = "If different to above";
            public const string TooLongErrorText = "Email address mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Email address " + Whitelists.EmailAddressWhitelist.ErrorText;
        }

        public static class SystemnameMessages
        {
            public const string LabelText = "System name";
            public const string HintText = "If different to above";
            public const string TooLongErrorText = "System name mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "System name" + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}