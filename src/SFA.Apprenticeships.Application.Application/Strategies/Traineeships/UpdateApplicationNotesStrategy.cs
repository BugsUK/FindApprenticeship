namespace SFA.Apprenticeships.Application.Application.Strategies.Traineeships
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class UpdateApplicationNotesStrategy : IUpdateApplicationNotesStrategy
    {
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;

        public UpdateApplicationNotesStrategy(ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository)
        {
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
        }

        public void UpdateApplicationNotes(Guid applicationId, string notes)
        {
            //_traineeshipApplicationWriteRepository.UpdateApplicationNotes(applicationId, notes);
        }
    }
}