namespace SFA.Apprenticeships.Application.Applications.Housekeeping
{
    using System.Collections.Generic;

    public interface IApplicationHousekeeper
    {
        IEnumerable<ApplicationHousekeepingRequest> GetHousekeepingRequests();
 
        void Handle(ApplicationHousekeepingRequest request);

        IApplicationHousekeeper Successor { get; set; }
    }
}