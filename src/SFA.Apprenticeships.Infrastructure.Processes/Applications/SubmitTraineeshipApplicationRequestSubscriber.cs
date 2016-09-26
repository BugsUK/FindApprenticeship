namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using System;
    using Application.Candidate;
    using Application.Interfaces;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class SubmitTraineeshipApplicationRequestSubscriber : IServiceBusSubscriber<SubmitTraineeshipApplicationRequest>
    {
        private readonly ILogService _logger;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeeshipApplicationWriteRepository;

        public SubmitTraineeshipApplicationRequestSubscriber(
            ILogService logger,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            ITraineeshipApplicationWriteRepository traineeeshipApplicationWriteRepository)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _traineeeshipApplicationWriteRepository = traineeeshipApplicationWriteRepository;
            _logger = logger;
        }

        [ServiceBusTopicSubscription(TopicName = "SubmitTraineeshipApplication")]
        public ServiceBusMessageStates Consume(SubmitTraineeshipApplicationRequest request)
        {
            return CreateApplication(request);
        }

        public ServiceBusMessageStates CreateApplication(SubmitTraineeshipApplicationRequest request)
        {
            _logger.Debug("Creating traineeship application Id: {0}", request.ApplicationId);

            var applicationDetail = _traineeshipApplicationReadRepository.Get(request.ApplicationId, true);

            try
            {
                SetApplicationStateSubmitted(applicationDetail);
                return ServiceBusMessageStates.Complete;
            }
            catch (Exception e)
            {
                _logger.Error("Submit traineeship application with Id = {0} request async process failed.",
                    e, request.ApplicationId);

                return ServiceBusMessageStates.Requeue;
            }
        }

        private void SetApplicationStateSubmitted(TraineeshipApplicationDetail traineeshipApplication)
        {
            traineeshipApplication.SetStateSubmitted();
            _traineeeshipApplicationWriteRepository.Save(traineeshipApplication);
        }
    }
}
