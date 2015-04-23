namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Candidates;

    public interface ICandidateReadRepository : IReadRepository<Candidate>
    {
        Candidate Get(Guid id, bool errorIfNotFound);
        Candidate Get(string username, bool errorIfNotFound = true);
        Candidate Get(int legacyCandidateId, bool errorIfNotFound = true);
        IEnumerable<Candidate> GetAllWith(string phoneNumber, bool errorIfNotFound = true);
    }

    public interface ICandidateWriteRepository : IWriteRepository<Candidate>
    {
    }
}