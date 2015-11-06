namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using System;
    using Domain.Entities.Vacancies.ProviderVacancies;

    public class VacancySummaryViewModel
    {
        public long VacancyReferenceNumber { get; set; }

        public string Title { get; set; }

        public ProviderVacancyStatuses Status { get; set; }

        public string ProviderName { get; set; }

        public DateTime ClosingDate { get; set; }

        public DateTime DateSubmitted { get; set; }

        public DateTime? DateStartedToQA { get; set; }
        
        public string QAUserName { get; set; }
    }
}