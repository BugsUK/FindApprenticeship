namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    public class TraineeshipApplicationSummary : ApplicationSummary
    {
        public TraineeshipApplicationSummary()
        {
            Status = ApplicationStatuses.Submitted;
        }
    }
}