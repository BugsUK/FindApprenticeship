namespace SFA.Apprenticeships.Application.Applications.Housekeeping
{
    public interface IRootApplicationHousekeeper
    {
        int QueueHousekeepingRequests();

        void Handle(ApplicationHousekeepingRequest request);
    }
}