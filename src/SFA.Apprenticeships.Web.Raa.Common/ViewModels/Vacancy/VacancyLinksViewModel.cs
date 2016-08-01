namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Vacancies;

    public class VacancyLinksViewModel
    {
        public const string PartialView = "DisplayTemplates/_VacancyLinks";

        public VacancyLinksViewModel(int vacancyReferenceNumber, VacancyStatus status, int totalNumberOfApplications)
        {
            VacancyReferenceNumber = vacancyReferenceNumber;
            Status = status;
            TotalNumberOfApplications = totalNumberOfApplications;
        }

        public int VacancyReferenceNumber { get; private set; }
        public VacancyStatus Status { get; private set; }
        public int TotalNumberOfApplications { get; private set; }
    }
}