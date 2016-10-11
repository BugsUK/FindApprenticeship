namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class ApiUserViewModelMessages
    {
        public const string CompanyIdAlreadyExists = "The supplied Company ID already has an API user associated with it";
        public const string UnknownCompanyId = "The supplied Company ID has not been found";
        public const string ApiUserCreationError = "An error occured when creating the api user. Please check your entries and try again";
        public const string ApiUserCreatedSuccessfully = "New api user added successfully";
        public const string ApiUserSavedSuccessfully = "The changes to the api user were saved successfully";
        public const string ApiUserSaveError = "An error occured when saving the api user. Please check your entries and try again";
        public const string ResetApiUserPasswordSuccessfully = "The api user's password was reset successfully";
        public const string ResetApiUserPasswordError = "An error occured when resetting the api user's password. Please try again";

        public class ExternalSystemId
        {
            public const string LabelText = "External System ID (Username. Optional. Will be created for you if not specified)";
        }

        public class Password
        {
            public const string LabelText = "Password (optional. Will be created for you if not specified)";
            public const string RequiredLengthErrorText = "Password must be 16 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Password " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public class CompanyId
        {
            public const string LabelText = "Company ID (UKPRN or EDSURN)";
            public const string RequiredErrorText = "Enter the companies ID";
            public const string RequiredLengthErrorText = "Company ID must be 8 or 9 digits";
            public const string WhiteListRegularExpression = Whitelists.NumericalWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Company ID " + Whitelists.NumericalWhitelist.ErrorText;
        }
    }
}