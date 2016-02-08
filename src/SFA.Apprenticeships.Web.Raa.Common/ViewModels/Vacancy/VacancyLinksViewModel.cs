namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Vacancies.ProviderVacancies;

    public class VacancyLinksViewModel
    {
        public const string PartialView = "DisplayTemplates/_VacancyLinks";

        public VacancyLinksViewModel(int vacancyReferenceNumber, ProviderVacancyStatuses status)
        {
            VacancyReferenceNumber = vacancyReferenceNumber;
            Status = status;
        }

        public int VacancyReferenceNumber { get; private set; }
        public ProviderVacancyStatuses Status { get; private set; }
    }
}