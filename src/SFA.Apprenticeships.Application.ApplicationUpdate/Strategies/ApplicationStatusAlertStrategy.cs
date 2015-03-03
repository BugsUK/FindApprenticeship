namespace SFA.Apprenticeships.Application.ApplicationUpdate.Strategies
{
    using Domain.Interfaces.Messaging;
    using Entities;

    public class ApplicationStatusAlertStrategy : IApplicationStatusAlertStrategy
    {
        private readonly IMessageBus _messageBus;
        public ApplicationStatusAlertStrategy(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void Send(ApplicationStatusSummary applicationStatusSummary)
        {
            //todo: 1.7: publish alert message for status change if this is a submitted application (i.e. becoming successful or unsuccessful)
            //note: only do this if processing an application etl/sync message (really should be separate messages)
            var applicationStatusChanged = new ApplicationStatusChanged {};

            _messageBus.PublishMessage(applicationStatusChanged);
        }
    }
}
