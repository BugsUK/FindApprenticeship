namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;

    public class ApprenticeshipApplicationSummary : ApplicationSummary
    {
        public string UnsuccessfulReason { get; set; }
        public DateTime UnsuccessfulDateTime { get; set; }
    }
}