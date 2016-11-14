namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyStatus
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
    }
}
