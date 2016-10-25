namespace SFA.Apprenticeships.Application.Application.Strategies.Apprenticeships
{
    using System;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;

    public class UpdateApplicationNotesStrategy : IUpdateApplicationNotesStrategy
    {
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly IServiceBus _serviceBus;

        public UpdateApplicationNotesStrategy(IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository, IServiceBus serviceBus)
        {
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _serviceBus = serviceBus;
        }

        public void UpdateApplicationNotes(Guid applicationId, string notes, bool publishUpdate)
        {
            _apprenticeshipApplicationWriteRepository.UpdateApplicationNotes(applicationId, notes);
            if (publishUpdate)
            {
                _serviceBus.PublishMessage(new ApprenticeshipApplicationUpdate(applicationId, ApplicationUpdateType.Update));
            }
        }
    }
}