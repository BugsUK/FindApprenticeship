namespace SFA.Apprenticeships.Application.Application.Strategies.Traineeships
{
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
    using System;

    public class SetApplicationStatusStrategy : ISetApplicationStatusStrategy
    {
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly IServiceBus _serviceBus;

        public SetApplicationStatusStrategy(
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository, IServiceBus serviceBus)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _serviceBus = serviceBus;
        }

        public void SetStateInProgress(Guid applicationId)
        {
            var application = _traineeshipApplicationReadRepository.Get(applicationId);
            application.SetStateInProgress();
            _traineeshipApplicationWriteRepository.Save(application);
            _serviceBus.PublishMessage(new TraineeshipApplicationUpdate(applicationId, ApplicationUpdateType.Update));
        }

        public void SetStateSubmitted(Guid applicationId)
        {
            var application = _traineeshipApplicationReadRepository.Get(applicationId);
            application.SetStateSubmitted();
            _traineeshipApplicationWriteRepository.Save(application);
            _serviceBus.PublishMessage(new TraineeshipApplicationUpdate(applicationId, ApplicationUpdateType.Update));
        }
    }
}