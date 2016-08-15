namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using ProviderUser;

    public class DeleteVacancyViewModel
    {
        public int VacancyId { get; set; }
        public string VacancyTitle { get; set; }
        public VacanciesSummarySearchViewModel VacanciesSummarySearch { get; set; }
    }
}