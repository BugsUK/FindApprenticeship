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
    }
}
