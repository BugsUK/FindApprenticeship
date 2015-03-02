namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using Entities;

    public class ApplicationStatusChangedStrategy : IApplicationStatusChangedStrategy
    {
        public void Send(ApplicationStatusSummary applicationStatusSummary)
        {
            //todo: 1.7: publish communication message for later processing. 
            //initially only interested in status changes of submitted applications (i.e. becoming successful or unsuccessful)
            //details TBC - may be removed and replaced with queue message...
        }
    }
}
