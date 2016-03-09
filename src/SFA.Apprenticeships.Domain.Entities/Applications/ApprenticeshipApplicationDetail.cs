namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;
    using Vacancies.Apprenticeships;

    public class ApprenticeshipApplicationDetail : ApplicationDetail
    {
        public ApprenticeshipApplicationDetail()
        {
            Vacancy = new ApprenticeshipSummary();
        }

        public DateTime? SuccessfulDateTime { get; set; }

        public DateTime? UnsuccessfulDateTime { get; set; }

        public string WithdrawnOrDeclinedReason { get; set; }

        public string UnsuccessfulReason { get; set; }

        public ApprenticeshipSummary Vacancy { get; set; }
    }
}
