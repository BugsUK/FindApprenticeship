namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Candidates;

    public interface ISavedSearchReadRepository : IReadRepository<SavedSearch> {
        IList<SavedSearch> GetForCandidate(Guid candidateId);
    }

    public interface ISavedSearchWriteRepository : IWriteRepository<SavedSearch> {}
}
