namespace SFA.Apprenticeships.Infrastructure.Processes.Vacancies
{
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Domain.Interfaces.Messaging;

    public class ApprenticeshipSummaryUpdateSubscriber : IServiceBusSubscriber<ApprenticeshipSummaryUpdate>
    {
        private readonly IApprenticeshipSummaryUpdateProcessor _apprenticeshipSummaryUpdateProcessor;

        public ApprenticeshipSummaryUpdateSubscriber(IApprenticeshipSummaryUpdateProcessor apprenticeshipSummaryUpdateProcessor)
        {
            _apprenticeshipSummaryUpdateProcessor = apprenticeshipSummaryUpdateProcessor;
        }

        [ServiceBusTopicSubscription(TopicName = "UpdateApprenticeshipSummary")]
        public ServiceBusMessageStates Consume(ApprenticeshipSummaryUpdate message)
        {
            _apprenticeshipSummaryUpdateProcessor.Process(message);

            return ServiceBusMessageStates.Complete;
        }
    }
}
