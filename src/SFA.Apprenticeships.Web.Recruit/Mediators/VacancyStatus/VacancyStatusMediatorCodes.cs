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
    }
}
