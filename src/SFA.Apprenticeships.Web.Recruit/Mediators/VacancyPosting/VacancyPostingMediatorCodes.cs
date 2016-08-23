namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    public static class VacancyPostingMediatorCodes
    {
        public static class GetVacancyParties
        {
            public const string Ok = "VacancyPosting.GetVacancyParties.Ok";
        }

        public static class GetProviderEmployers
        {
            public const string Ok = "VacancyPosting.GetProviderEmployers.Ok";
            public const string NoResults = "VacancyPosting.GetProviderSiteEmployers.NoResults";
            public const string FailedValidation = "VacancyPosting.GetProviderSiteEmployers.FailedValidation";
        }

        public static class GetEmployers
        {
            public const string Ok = "VacancyPosting.GetEmployers.Ok";
        }

        public static class GetEmployer
        {
            public const string Ok = "VacancyPosting.GetEmployer.Ok";
            public const string InvalidEmployerAddress = "VacancyPosting.GetEmployer.InvalidEmployerAddress";
        }

        public static class ConfirmEmployer
        {
            public const string Ok = "VacancyPosting.ConfirmEmployer.Ok";
            public const string FailedValidation = "VacancyPosting.ConfirmEmployer.FailedValidation";
        }

        public static class GetNewVacancyViewModel
        {
            public const string Ok = "VacancyPosting.GetNewVacancyViewModel.Ok";
            public const string FailedValidation = "VacancyPosting.GetNewVacancyViewModel.FailedValidation";
            public const string LocationNotSet = "VacancyPosting.GetNewVacancyViewModel.LocationNotSet";
        }

        public static class GetVacancyViewModel
        {
            public const string Ok = "VacancyPosting.GetVacancyViewModel.Ok";
            public const string FailedValidation = "VacancyPosting.GetVacancyViewModel.FailedValidation";
        }

        public static class GetPreviewVacancyViewModel
        {
            public const string Ok = "VacancyPosting.GetPreviewVacancyViewModel.Ok";
            public const string FailedValidation = "VacancyPosting.GetPreviewVacancyViewModel.FailedValidation";
        }

        public static class GetTrainingDetailsViewModel
        {
            public const string Ok = "VacancyPosting.GetTrainingDetailsViewModel.Ok";
            public const string FailedValidation = "VacancyPosting.GetTrainingDetailsViewModel.FailedValidation";
        }

        public static class GetVacancySummaryViewModel
        {
            public const string Ok = "VacancyPosting.GetVacancySummaryViewModel.Ok";
            public const string FailedValidation = "VacancyPosting.GetVacancySummaryViewModel.FailedValidation";
        }

        public static class GetVacancyRequirementsProspectsViewModel
        {
            public const string Ok = "VacancyPosting.GetVacancyRequirementsProspectsViewModel.Ok";
            public const string FailedValidation = "VacancyPosting.GetVacancyRequirementsProspectsViewModel.FailedValidation";
        }

        public static class GetVacancyQuestionsViewModel
        {
            public const string Ok = "VacancyPosting.GetVacancyQuestionsViewModel.Ok";
            public const string FailedValidation = "VacancyPosting.GetVacancyQuestionsViewModel.FailedValidation";
        }

        public static class CreateVacancy
        {
            public const string Ok = "VacancyPosting.CreateVacancy.Ok";
            public const string OkWithWarning = "VacancyPosting.CreateVacancy.OkWithWarning";
            public const string FailedValidation = "VacancyPosting.CreateVacancy.FailedValidation";
        }

        public static class UpdateVacancy
        {
            public const string Ok = "VacancyPosting.UpdateVacancy.Ok";
            public const string OkAndExit = "VacancyPosting.UpdateVacancy.OkAndExit";
            public const string OfflineVacancyOk = "VacancyPosting.UpdateVacancy.OfflineVacancyOk";
            public const string OnlineVacancyOk = "VacancyPosting.UpdateVacancy.OnlineVacancyOk";
            public const string FailedValidation = "VacancyPosting.UpdateVacancy.FailedValidation";
        }
        
        public static class SubmitVacancy
        {
            public const string SubmitOk = "VacancyPosting.SubmitVacancy.SubmitOk";
            public const string ResubmitOk = "VacancyPosting.SubmitVacancy.ResubmitOk";
            public const string FailedValidation = "VacancyPosting.SubmitVacancy.FailedValidation";
        }

        public static class GetSubmittedVacancyViewModel
        {
            public const string Ok = "VacancyPosting.GetSubmittedVacancyViewModel.Ok";
        }

        public class SelectNewEmployer
        {
            public const string Ok = "VacancyPosting.SelectNewEmployer.Ok";
            public const string FailedValidation = "VacancyPosting.SelectNewEmployer.FailedValidation";
            public const string NoResults = "VacancyPosting.SelectNewEmployer.NoResults";
        }

        public class CloneVacancy
        {
            public const string Ok = "VacancyPosting.CloneVacancy.Ok";
            public const string VacancyInIncorrectState = "VacancyPosting.CloneVacancy.Failure";
        }

        public class GetLocationAddressesViewModel
        {
            public const string Ok = "VacancyPosting.GetLocationAddressesViewModel.Ok";
        }

        public class SetDifferentLocation
        {
            public const string Ok = "VacancyPosting.SetDifferentLocation.Ok";
        }

        public class SearchLocations
        {
            public const string Ok = "VacancyPosting.SearchLocations.Ok";
            public const string NotFullPostcode = "VacancyPosting.SearchLocations.NotFullPostcode";
        }

        public class UseLocation
        {
            public const string Ok = "VacancyPosting.UseLocation.Ok";
        }

        public class RemoveLocation
        {
            public const string Ok = "VacancyPosting.RemoveLocation.Ok";
        }

        public class ClearLocationInformation
        {
            public const string Ok = "VacancyPosting.ClearLocationInformation.Ok";
        }

        public class ManageDates
        {
            public const string Ok = "VacancyPosting.ManageDates.Ok";
            public const string UpdatedHasApplications = "VacancyPosting.ManageDates.UpdatedHasApplications";
            public const string UpdatedNoApplications = "VacancyPosting.ManageDates.OkNoApplications";
            public const string FailedValidation = "VacancyPosting.ManageDates.FailedValidation";
            public const string InvalidState = "VacancyPosting.ManageDates.InvalidState";
        }
    }
}
