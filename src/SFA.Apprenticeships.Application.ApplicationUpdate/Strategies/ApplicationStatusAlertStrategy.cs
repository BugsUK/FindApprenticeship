namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using Domain.Entities.Communication;
    using Entities;

    public class ApplicationStatusAlertStrategy : IApplicationStatusAlertStrategy
    {
        public void Send(ApplicationStatusSummary applicationStatusSummary)
        {
            //todo: 1.7: publish alert message for status change if this is a submitted application (i.e. becoming successful or unsuccessful)
            //note: only do this if 
            var alert = new ApplicationStatusAlert
            {
                //
            };
        }
    }
}
