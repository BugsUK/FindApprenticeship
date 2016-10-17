namespace SFA.Apprenticeships.Application.Application.Strategies.Apprenticeships
{
    using Domain.Interfaces.Repositories;
    using System;

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