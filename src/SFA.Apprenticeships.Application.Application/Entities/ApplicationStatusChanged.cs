namespace SFA.Apprenticeships.Application.Application.Entities
{
    using Domain.Entities.Applications;

    public class ApplicationStatusChanged
    {
        public int LegacyApplicationId { get; set; }

        public ApplicationStatuses ApplicationStatus { get; set; }

        public string UnsuccessfulReason { get; set; }
    }
}
