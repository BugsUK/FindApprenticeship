namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    public class CandidateRepository : ICandidateReadRepository
    {
        public Candidate Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Candidate Get(Guid id, bool errorIfNotFound)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Candidate> Get(string username, bool errorIfNotFound = true)
        {
            throw new NotImplementedException();
        }

        public Candidate Get(int legacyCandidateId, bool errorIfNotFound = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Candidate> GetAllCandidatesWithPhoneNumber(string phoneNumber, bool errorIfNotFound = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> GetCandidatesWithPendingMobileVerification()
        {
            throw new NotImplementedException();
        }

        public Candidate GetBySubscriberId(Guid subscriberId, bool errorIfNotFound = true)
        {
            throw new NotImplementedException();
        }

        public List<CandidateSummary> SearchCandidates(CandidateSearchRequest request)
        {
            throw new NotImplementedException();
        }
    }
}