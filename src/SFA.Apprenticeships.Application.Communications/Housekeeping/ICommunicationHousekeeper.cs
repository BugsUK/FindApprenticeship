namespace SFA.Apprenticeships.Application.Communications.Housekeeping
{
    using System.Collections.Generic;

    public interface ICommunicationHousekeeper
    {
        IEnumerable<CommunicationHousekeepingRequest> GetHousekeepingRequests();

        void Handle(CommunicationHousekeepingRequest request);

        ICommunicationHousekeeper Successor { get; set; }
    }
}