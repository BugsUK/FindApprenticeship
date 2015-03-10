namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Candidates;

    //todo: 1.8: repo for candidate's saved searches
    public interface ISavedSearchReadRepository : IReadRepository<SavedSearch> {}

    public interface ISavedSearchWriteRepository : IWriteRepository<SavedSearch> {}
}
