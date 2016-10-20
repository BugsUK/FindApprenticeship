namespace SFA.Apprenticeships.Application.Application.Strategies.Traineeships
{
    using System;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;

    public class UpdateApplicationNotesStrategy : IUpdateApplicationNotesStrategy
    {
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly IServiceBus _serviceBus;

        public UpdateApplicationNotesStrategy(ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository, IServiceBus serviceBus)
        {
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _serviceBus = serviceBus;
        }

        public void UpdateApplicationNotes(Guid applicationId, string notes, bool publishUpdate)
        {
            _traineeshipApplicationWriteRepository.UpdateApplicationNotes(applicationId, notes);
            if (publishUpdate)
            {
                _serviceBus.PublishMessage(new TraineeshipApplicationUpdate(applicationId));
            }
        }
    }
}