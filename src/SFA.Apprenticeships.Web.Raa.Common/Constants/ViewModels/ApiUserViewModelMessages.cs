namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class ApiUserViewModelMessages
    {
        public const string CompanyIdAlreadyExists = "The supplied Company ID already has an API user associated with it";
        public const string ApiUserCreatedSuccessfully = "New api user added successfully";
        public const string ApiUserSavedSuccessfully = "The changes to the api user were saved successfully";
        public const string ApiUserSaveError = "An error occured when saving the api user. Please check your entries and try again";

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