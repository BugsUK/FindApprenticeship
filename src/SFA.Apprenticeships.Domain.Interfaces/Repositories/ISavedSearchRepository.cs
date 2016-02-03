namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Candidates;

    public interface ISavedSearchReadRepository : IReadRepository<SavedSearch, Guid> {
        IList<SavedSearch> GetForCandidate(Guid candidateId);

        IEnumerable<Guid> GetCandidateIds();
    }

    public interface ISavedSearchWriteRepository : IWriteRepository<SavedSearch, Guid> {}
}
