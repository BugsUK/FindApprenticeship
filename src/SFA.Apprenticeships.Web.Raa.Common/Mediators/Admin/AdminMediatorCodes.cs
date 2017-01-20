namespace SFA.Apprenticeships.Web.Raa.Common.Mediators.Admin
{
    public class AdminMediatorCodes
    {
        public class SearchProviders
        {
            public const string FailedValidation = "AdminMediatorCodes.SearchProviders.FailedValidation";
            public const string Ok = "AdminMediatorCodes.SearchProviders.Ok";
        }

        public class GetProvider
        {
            public const string Ok = "AdminMediatorCodes.GetProvider.Ok";
        }

        public class CreateProvider
        {
            public const string FailedValidation = "AdminMediatorCodes.CreateProvider.FailedValidation";
            public const string UkprnAlreadyExists = "AdminMediatorCodes.CreateProvider.UkprnAlreadyExists";
            public const string Ok = "AdminMediatorCodes.CreateProvider.Ok";
        }

        public class SaveProvider
        {
            public const string FailedValidation = "AdminMediatorCodes.SaveProvider.FailedValidation";
            public const string Error = "AdminMediatorCodes.SaveProvider.Error";
            public const string Ok = "AdminMediatorCodes.SaveProvider.Ok";
        }

        public class SearchProviderSites
        {
            public const string FailedValidation = "AdminMediatorCodes.SearchProviderSites.FailedValidation";
            public const string Ok = "AdminMediatorCodes.SearchProviderSites.Ok";
        }

        public class GetProviderSite
        {
            public const string Ok = "AdminMediatorCodes.GetProviderSite.Ok";
        }

        public class CreateProviderSite
        {
            public const string FailedValidation = "AdminMediatorCodes.CreateProviderSite.FailedValidation";
            public const string EdsUrnAlreadyExists = "AdminMediatorCodes.CreateProviderSite.EdsUrnAlreadyExists";
            public const string Ok = "AdminMediatorCodes.CreateProviderSite.Ok";
        }

        public class SaveProviderSite
        {
            public const string FailedValidation = "AdminMediatorCodes.SaveProviderSite.FailedValidation";
            public const string Error = "AdminMediatorCodes.SaveProviderSite.Error";
            public const string Ok = "AdminMediatorCodes.SaveProviderSite.Ok";
        }

        public class GetProviderSiteRelationship
        {
            public const string Error = "AdminMediatorCodes.GetProviderSiteRelationship.Error";
            public const string Ok = "AdminMediatorCodes.GetProviderSiteRelationship.Ok";
        }

        public class CreateProviderSiteRelationship
        {
            public const string FailedValidation = "AdminMediatorCodes.CreateProviderSiteRelationship.FailedValidation";
            public const string InvalidUkprn = "AdminMediatorCodes.CreateProviderSiteRelationship.InvalidUkprn";
            public const string Error = "AdminMediatorCodes.CreateProviderSiteRelationship.Error";
            public const string Ok = "AdminMediatorCodes.CreateProviderSiteRelationship.Ok";
        }


        public class DeleteProviderSiteRelationship
        {
            public const string Error = "AdminMediatorCodes.DeleteProviderSiteRelationship.Error";
            public const string Ok = "AdminMediatorCodes.DeleteProviderSiteRelationship.Ok";
        }

        public class SearchApiUsers
        {
            public const string FailedValidation = "AdminMediatorCodes.SearchApiUsers.FailedValidation";
            public const string Ok = "AdminMediatorCodes.SearchApiUsers.Ok";
        }

        public class GetApiUser
        {
            public const string Ok = "AdminMediatorCodes.GetApiUser.Ok";
        }

        public class CreateApiUser
        {
            public const string FailedValidation = "AdminMediatorCodes.CreateApiUser.FailedValidation";
            public const string CompanyIdAlreadyExists = "AdminMediatorCodes.CreateApiUser.CompanyIdAlreadyExists";
            public const string UnknownCompanyId = "AdminMediatorCodes.CreateApiUser.UnknownCompanyId";
            public const string Error = "AdminMediatorCodes.CreateApiUser.Error";
            public const string Ok = "AdminMediatorCodes.CreateApiUser.Ok";
        }

        public class SaveApiUser
        {
            public const string FailedValidation = "AdminMediatorCodes.SaveApiUser.FailedValidation";
            public const string Error = "AdminMediatorCodes.SaveApiUser.Error";
            public const string Ok = "AdminMediatorCodes.SaveApiUser.Ok";
        }

        public class ResetApiUserPassword
        {
            public const string FailedValidation = "AdminMediatorCodes.ResetApiUserPassword.FailedValidation";
            public const string Error = "AdminMediatorCodes.ResetApiUserPassword.Error";
            public const string Ok = "AdminMediatorCodes.ResetApiUserPassword.Ok";
        }

        public class GetApiUsersBytes
        {
            public const string FailedValidation = "AdminMediatorCodes.GetApiUsersBytes.FailedValidation";
            public const string Error = "AdminMediatorCodes.GetApiUsersBytes.Error";
            public const string Ok = "AdminMediatorCodes.GetApiUsersBytes.Ok";
        }

        public class GetVacancyDetails
        {
            public const string NoRecordsFound = "AdminMediatorCodes.GetVacancyDetails.NoRecordsFound";
            public const string FailedAuthorisation = "AdminMediatorCodes.GetVacancyDetails.FailedAuthorisation";
            public const string Ok = "AdminMediatorCodes.GetVacancyDetails.Ok";
            public const string FailedTransfer = "AdminMediatorCodes.GetVacancyDetails.FailedException";
        }

        public class TransferVacancy
        {
            public const string FailedTransfer = "AdminMediatorCodes.TransferVacancy.FailedTransfer";
            public const string Ok = "AdminMediatorCodes.TransferVacancy.Ok";
        }

        public class GetProviderUsers
        {
            public const string Ok = "AdminMediatorCodes.GetProviderUsers.Ok";
        }

        public class SearchProviderUsers
        {
            public const string FailedValidation = "AdminMediatorCodes.FailedValidation.Ok";
            public const string Ok = "AdminMediatorCodes.SearchProviderUsers.Ok";
        }

        public class GetProviderUser
        {
            public const string Ok = "AdminMediatorCodes.GetProviderUser.Ok";
        }

        public class SaveProviderUser
        {
            public const string FailedValidation = "AdminMediatorCodes.SaveProviderUser.FailedValidation";
            public const string Error = "AdminMediatorCodes.SaveProviderUser.Error";
            public const string Ok = "AdminMediatorCodes.SaveProviderUser.Ok";
        }

        public class VerifyProviderUserEmail
        {
            public const string Error = "AdminMediatorCodes.VerifyProviderUserEmail.Error";
            public const string Ok = "AdminMediatorCodes.VerifyProviderUserEmail.Ok";
        }

        public class SearchEmployers
        {
            public const string FailedValidation = "AdminMediatorCodes.SearchEmployers.FailedValidation";
            public const string Ok = "AdminMediatorCodes.SearchEmployers.Ok";
        }

        public class GetEmployer
        {
            public const string Ok = "AdminMediatorCodes.GetEmployer.Ok";
        }

        public class SaveEmployer
        {
            public const string FailedValidation = "AdminMediatorCodes.SaveEmployer.FailedValidation";
            public const string Error = "AdminMediatorCodes.SaveEmployer.Error";
            public const string Ok = "AdminMediatorCodes.SaveEmployer.Ok";
        }

        public class GetStandard
        {
            public const string Ok = "AdminMediatorCodes.GetStandard.Ok";
        }

        public class GetStandardsBytes
        {
            public const string FailedValidation = "AdminMediatorCodes.GetStandardsBytes.FailedValidation";
            public const string Error = "AdminMediatorCodes.GetStandardsBytes.Error";
            public const string Ok = "AdminMediatorCodes.GetStandardsBytes.Ok";
        }

        public class GetFrameworks
        {
            public const string Ok = "AdminMediatorCodes.GetFrameworks.Ok";
        }

        public class GetFrameworksBytes
        {
            public const string FailedValidation = "AdminMediatorCodes.GetFrameworksBytes.FailedValidation";
            public const string Error = "AdminMediatorCodes.GetFrameworksBytes.Error";
            public const string Ok = "AdminMediatorCodes.GetFrameworksBytes.Ok";
        }

        public class UpdateStandard
        {
            public const string Ok = "AdminMediatorCodes.UpdateStandard.Ok";
        }
    }
}