namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Candidate
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;

    public class CandidateApprenticeshipApplicationViewModel
    {
        public CandidateApprenticeshipApplicationViewModel()
        {    
        }

        public CandidateApprenticeshipApplicationViewModel(ApprenticeshipApplicationSummary applicationSummary)
        {
            ApplicationId = applicationSummary.ApplicationId;
            VacancyId = applicationSummary.LegacyVacancyId;
            Title = applicationSummary.Title;
            EmployerName = applicationSummary.EmployerName;
            
            UnsuccessfulReason = applicationSummary.UnsuccessfulReason;
            ApplicationStatus = applicationSummary.Status;
            VacancyStatus = applicationSummary.VacancyStatus;
            IsArchived = applicationSummary.IsArchived;
            DateApplied = applicationSummary.DateApplied;
            ClosingDate = applicationSummary.ClosingDate;
            DateUpdated = applicationSummary.DateUpdated;
            IsPositiveAboutDisability = applicationSummary.IsPositiveAboutDisability;
        }

        public Guid ApplicationId { get; set; }

        public int VacancyId { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public bool IsPositiveAboutDisability { get; set; }

        public ApplicationStatuses ApplicationStatus { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public string ApplicationStatusDescription
        {
            get
            {
                switch (ApplicationStatus)
                {
                    case ApplicationStatuses.Draft:
                        return "Draft";

                    case ApplicationStatuses.Submitted:
                    case ApplicationStatuses.Submitting:
                        return "Submitted";

                    case ApplicationStatuses.InProgress:
                        return "In progress";

                    case ApplicationStatuses.ExpiredOrWithdrawn:
                        return "Expired or withdrawn";

                    case ApplicationStatuses.Successful:
                        return "Successful";

                    case ApplicationStatuses.Unsuccessful:
                        return "Unsuccessful";
                }

                return string.Empty;
            }
        }

        public bool IsArchived { get; set; }

        public DateTime? DateApplied { get; set; }

        public DateTime ClosingDate { get; set; }

        public DateTime DateUpdated { get; set; }

        public string UnsuccessfulReason { get; set; }
    }
}
