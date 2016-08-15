namespace SFA.Apprenticeships.Infrastructure.Processes.Vacancies
{
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Domain.Interfaces.Messaging;

    public class TraineeshipsSummaryUpdateSubscriber : IServiceBusSubscriber<TraineeshipSummaryUpdate>
    {
        private readonly ITraineeshipsSummaryUpdateProcessor _traineeshipsSummaryUpdateProcessor;

        public TraineeshipsSummaryUpdateSubscriber(ITraineeshipsSummaryUpdateProcessor traineeshipsSummaryUpdateProcessor)
        {
            _traineeshipsSummaryUpdateProcessor = traineeshipsSummaryUpdateProcessor;
        }

        [ServiceBusTopicSubscription(TopicName = "UpdateTraineeshipSummary")]
        public ServiceBusMessageStates Consume(TraineeshipSummaryUpdate message)
        {
            _traineeshipsSummaryUpdateProcessor.Process(message);

            return ServiceBusMessageStates.Complete;
        }
    }
}
