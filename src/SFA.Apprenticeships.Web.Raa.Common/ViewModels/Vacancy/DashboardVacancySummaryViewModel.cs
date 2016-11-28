namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using System;
    using Web.Common.ViewModels.Locations;

    public class DashboardVacancySummaryViewModel
    {
        public int VacancyReferenceNumber { get; set; }

        public string Title { get; set; }

        public VacancyStatus Status { get; set; }

        public string ProviderName { get; set; }

        public DateTime? ClosingDate { get; set; }

        public DateTime? DateSubmitted { get; set; }

        public DateTime? DateFirstSubmitted { get; set; }

        public DateTime? DateStartedToQA { get; set; }

        public string QAUserName { get; set; }

        public bool CanBeReservedForQaByCurrentUser { get; set; }

        public int SubmissionCount { get; set; }

        public VacancyType VacancyType { get; set; }
        public AddressViewModel Location { get; set; }
        public VacancyViewModel VacancyViewModel { get; set; }
    }
}