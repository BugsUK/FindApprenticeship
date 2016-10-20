namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Vacancies;

    public class VacancyLinksViewModel
    {
        public const string PartialView = "DisplayTemplates/_VacancyLinks";

        public VacancyLinksViewModel(int vacancyReferenceNumber,
            VacancyStatus status, int totalNumberOfApplications, VacancyType vacancyType, int totalApplicationsRequiringDecision)
        {
            VacancyReferenceNumber = vacancyReferenceNumber;
            Status = status;
            TotalNumberOfApplications = totalNumberOfApplications;
            VacancyType = vacancyType;
            TotalApplicationsRequiringDecision == totalApplicationsRequiringDecision;
        }

        public int VacancyReferenceNumber { get; private set; }
        public VacancyStatus Status { get; private set; }
        public int TotalNumberOfApplications { get; private set; }
        public VacancyType VacancyType { get; private set; }
        public int TotalApplicationsRequiringDecision { get; private set; }
    }
}