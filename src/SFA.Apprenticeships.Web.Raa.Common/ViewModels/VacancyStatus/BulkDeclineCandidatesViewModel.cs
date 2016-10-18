namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyStatus
{
    using Application;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using System;
    using System.Collections.Generic;

    public class BulkDeclineCandidatesViewModel
    {
        public BulkDeclineCandidatesViewModel(IEnumerable<ApplicationSummary> applicationSummaries, int vacancyId, int vacancyReferenceNumber)
        {
            if (vacancyId == 0 || vacancyReferenceNumber == 0)
            {
                throw new ArgumentNullException();
            }

            VacancyReferenceNumber = vacancyReferenceNumber;
            ApplicationSummaries = applicationSummaries;
            VacancyId = vacancyId;
        }
        public int VacancyId;
        public string VacancyTitle { get; set; }
        public string EmployerName { get; set; }
        public int VacancyReferenceNumber { get; set; }
        public VacancyType VacancyType { get; set; }
        public List<ApplicationSummaryViewModel> ApplicationSummariesViewModel { get; set; }
        public IEnumerable<ApplicationSummary> ApplicationSummaries { get; set; }
        public VacancyApplicationsSearchViewModel VacancyApplicationsSearch { get; set; }
        public int NewApplicationsCount { get; set; }
        public int InProgressApplicationsCount { get; set; }
        public bool CanBulkDeclineCandidates { get; set; }
    }
}