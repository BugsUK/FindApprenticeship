namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Vacancies.ProviderVacancies;

    public class VacancyLinksViewModel
    {
        public const string PartialView = "DisplayTemplates/_VacancyLinks";

        public VacancyLinksViewModel(long vacancyReferenceNumber, ProviderVacancyStatuses status)
        {
            VacancyReferenceNumber = vacancyReferenceNumber;
            Status = status;
        }

        public long VacancyReferenceNumber { get; private set; }
        public ProviderVacancyStatuses Status { get; private set; }
    }
}