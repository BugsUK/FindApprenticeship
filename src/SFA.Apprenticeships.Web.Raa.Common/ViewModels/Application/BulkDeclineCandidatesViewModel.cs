namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using Domain.Entities.Raa.Vacancies;
    using System;
    using System.Collections.Generic;
    using Web.Common.ViewModels;

    public class BulkDeclineCandidatesViewModel
    {
        public int VacancyId;
        public string VacancyTitle { get; set; }
        public string EmployerName { get; set; }
        public int VacancyReferenceNumber { get; set; }
        public VacancyType VacancyType { get; set; }
        public PageableViewModel<ApplicationSummaryViewModel> ApplicationSummariesViewModel { get; set; }
        public VacancyApplicationsSearchViewModel VacancyApplicationsSearch { get; set; }
        public int NewApplicationsCount { get; set; }
        public int InProgressApplicationsCount { get; set; }
        public bool IsSelected { get; set; }
        public IEnumerable<Guid> SelectedApplicationIds { get; set; }
    }
}