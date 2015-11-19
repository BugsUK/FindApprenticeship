namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    public static class VacancyPostingMediatorCodes
    {
        public static class GetProviderSiteEmployerLinks
        {
            public const string Ok = "VacancyPosting.GetProviderSiteEmployerLinks.Ok";
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
        }

        public static class ConfirmEmployer
        {
            public const string Ok = "VacancyPosting.ConfirmEmployer.Ok";
            public const string FailedValidation = "VacancyPosting.ConfirmEmployer.FailedValidation";
        }

        public static class GetNewVacancyModel
        {
            public const string Ok = "VacancyPosting.GetNewVacancyModel.Ok";
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

        public static class GetVacancySummaryViewModel
        {
            public const string Ok = "VacancyPosting.GetVacancySummaryViewModel.Ok";
        }

        public static class GetVacancyRequirementsProspectsViewModel
        {
            public const string Ok = "VacancyPosting.GetVacancyRequirementsProspectsViewModel.Ok";
        }

        public static class GetVacancyQuestionsViewModel
        {
            public const string Ok = "VacancyPosting.GetVacancyQuestionsViewModel.Ok";
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
            public const string Ok = "VacancyPosting.SubmitVacancy.Ok";
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

        public class CLoneVacancy
        {
            public const string Ok = "VacancyPosting.CloneVacancy.Ok";
        }
    }
}
