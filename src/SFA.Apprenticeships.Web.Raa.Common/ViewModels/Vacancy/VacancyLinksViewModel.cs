namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using Web.Common.ViewModels;

    public class VacancyLinksViewModel
    {
        public const string PartialView = "DisplayTemplates/_VacancyLinks";

        public VacancyLinksViewModel(int vacancyReferenceNumber,
            VacancyStatus status, int totalNumberOfApplications, VacancyType vacancyType, int totalNumberOfApplicationsWithPendingDecision, WageViewModel wage)
        {
            VacancyReferenceNumber = vacancyReferenceNumber;
            Status = status;
            TotalNumberOfApplications = totalNumberOfApplications;
            VacancyType = vacancyType;
            TotalNumberOfApplicationsWithPendingDecision = totalNumberOfApplicationsWithPendingDecision;
            Wage = wage;
        }

        public int VacancyReferenceNumber { get; private set; }
        public VacancyStatus Status { get; private set; }
        public int TotalNumberOfApplications { get; private set; }
        public VacancyType VacancyType { get; private set; }
        public int TotalNumberOfApplicationsWithPendingDecision { get; private set; }
        public WageViewModel Wage { get; private set; }
    }
}