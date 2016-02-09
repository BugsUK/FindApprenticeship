namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Vacancies;

    public class SubmittedVacancyViewModel
    {
        public long VacancyReferenceNumber { get; set; }
        public string ProviderSiteErn { get; set; }
        public bool Resubmitted { get; set; }
        public string VacancyText { get; set; }
        public bool IsMultiLocationVacancy { get; set; }
        public VacancyType VacancyType { get; set; }
    }
}