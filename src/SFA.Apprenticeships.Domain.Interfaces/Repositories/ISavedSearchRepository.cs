namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Candidates;

    public interface ISavedSearchReadRepository : IReadRepository<SavedSearch> {}

    public interface ISavedSearchWriteRepository : IWriteRepository<SavedSearch> {}
}
