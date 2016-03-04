namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Vacancies;

    public class ApplicationsLinkViewModel
    {
        public const string PartialView = "_ApplicationsLink";

        public ApplicationsLinkViewModel(VacancyType vacancyType, int vacancyReferenceNumber, int applicationCount)
        {
            VacancyType = vacancyType;
            VacancyReferenceNumber = vacancyReferenceNumber;
            ApplicationCount = applicationCount;
        }

        public VacancyType VacancyType { get; private set; }
        public int VacancyReferenceNumber { get; private set; }
        public int ApplicationCount { get; private set; }
    }
}
