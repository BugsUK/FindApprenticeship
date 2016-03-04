namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;
    using Users;
    using Vacancies;

    public abstract class ApplicationSummary
    {
        protected ApplicationSummary()
        {
            CandidateDetails = new RegistrationDetails();
        }

        public Guid ApplicationId { get; set; }

        public Guid CandidateId { get; set; }

        public RegistrationDetails CandidateDetails { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public ApplicationStatuses Status { get; set; }

        public int LegacyVacancyId { get; set; }

        public int LegacyApplicationId { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public bool IsPositiveAboutDisability { get; set; }

        public DateTime ClosingDate { get; set; }

        public bool IsArchived { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime? DateApplied { get; set; }

        public string Notes { get; set; }
    }
}
