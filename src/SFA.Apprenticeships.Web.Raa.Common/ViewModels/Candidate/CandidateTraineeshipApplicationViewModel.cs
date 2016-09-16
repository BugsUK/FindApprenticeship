namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Candidate
{
    using System;
    using Domain.Entities.Vacancies;

    public class CandidateTraineeshipApplicationViewModel
    {
        public Guid ApplicationId { get; set; }

        public int VacancyId { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public bool IsArchived { get; set; }

        public DateTime? DateApplied { get; set; }
    }
}