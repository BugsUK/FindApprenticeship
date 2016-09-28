namespace SFA.Apprenticeships.Web.Manage.Mediators.Admin
{
    public class AdminMediatorCodes
    {
        public class SearchProviders
        {
            public const string FailedValidation = "AdminMediatorCodes.SearchProviders.FailedValidation";
            public const string Ok = "AdminMediatorCodes.SearchProviders.Ok";
        }

        public class AddProvider
        {
            public const string FailedValidation = "AdminMediatorCodes.AddProvider.FailedValidation";
            public const string UkprnAlreadyExists = "AdminMediatorCodes.UkprnAlreadyExists.FailedValidation";
            public const string Ok = "AdminMediatorCodes.AddProvider.Ok";
        }
    }
}