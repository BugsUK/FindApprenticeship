namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Candidate
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Vacancies;

    public class CandidateApplicationSummaryViewModel
    {
        public Guid ApplicationId { get; set; }

        public int VacancyId { get; set; }

        public int VacancyReferenceNumber { get; set; }

        public string VacancyTitle { get; set; }

        public string EmployerName { get; set; }

        public string EmployerLocation { get; set; }

        public DateTime DateApplied { get; set; }

        public ApplicationStatuses Status { get; set; }

        public VacancyType VacancyType { get; set; }

        public string AnonymousLinkData { get; set; }
    }
}