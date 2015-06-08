namespace SFA.Apprenticeships.Application.Communications.Housekeeping
{
    public interface IRootCommunicationHousekeeper
    {
        int QueueHousekeepingRequests();

        void Handle(CommunicationHousekeepingRequest request);
    }
}