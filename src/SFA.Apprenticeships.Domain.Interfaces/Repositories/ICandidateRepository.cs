namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Candidates;

    public interface ICandidateReadRepository : IReadRepository<Candidate, Guid>
    {
        Candidate Get(Guid id, bool errorIfNotFound);

        IEnumerable<Candidate> Get(string username, bool errorIfNotFound = true);

        Candidate Get(int legacyCandidateId, bool errorIfNotFound = true);
        
        IEnumerable<Candidate> GetAllCandidatesWithPhoneNumber(string phoneNumber, bool errorIfNotFound = true);

        IEnumerable<Guid> GetCandidatesWithPendingMobileVerification();

        Candidate GetBySubscriberId(Guid subscriberId, bool errorIfNotFound = true);
    }

    public interface ICandidateWriteRepository : IWriteRepository<Candidate, Guid>
    {
    }
}