namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    public static class VacancyStatusMediatorCodes
    {
        public static class GetArchiveVacancyViewModel
        {
            public const string Ok = "VacancyPosting.GetArchiveVacancyViewModelByVacancyReferenceNumber.Ok";
        }

        public static class ArchiveVacancy
        {
            public const string Ok = "VacancyPosting.ArchiveVacancy.Ok";
            public const string OutstandingActions = "VacancyPosting.ArchiveVacancy.OutstandingActions";
        }

        public static class BulkDeclineCandidatesViewModel
        {
            public const string Ok = "VacancyPosting.BulkDeclineCandidatesViewModel.Ok";
            public const string OutstandingActions = "VacancyPosting.BulkDeclineCandidatesViewModel.OutstandingActions";
        }

        public static class BulkApplicationsReject
        {
            public const string Ok = "VacancyPosting.BulkApplicationsReject.Ok";
            public const string FailedValidation = "VacancyPosting.BulkApplicationsReject.FailedValidation";
        }

        public static class GetBulkDeclineCandidatesViewModel
        {
            public const string Ok = "VacancyPosting.BulkApplicationsReject.Ok";
        }
    }
}
