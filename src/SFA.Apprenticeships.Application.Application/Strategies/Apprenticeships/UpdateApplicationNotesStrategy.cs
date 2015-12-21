namespace SFA.Apprenticeships.Application.Application.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class UpdateApplicationNotesStrategy : IUpdateApplicationNotesStrategy
    {
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;

        public UpdateApplicationNotesStrategy(IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository)
        {
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
        }

        public void UpdateApplicationNotes(Guid applicationId, string notes)
        {
            _apprenticeshipApplicationWriteRepository.UpdateApplicationNotes(applicationId, notes);
        }
    }
}