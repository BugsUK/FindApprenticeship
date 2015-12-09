namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    public class SubmittedVacancyViewModel
    {
        public long VacancyReferenceNumber { get; set; }
        public string ProviderSiteErn { get; set; }
        public bool Resubmitted { get; set; }
    }
}