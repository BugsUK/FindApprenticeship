namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Candidates;

    public interface ICandidateReadRepository : IReadRepository<Candidate>
    {
        Candidate Get(Guid id, bool errorIfNotFound);

        IEnumerable<Candidate> Get(string username, bool errorIfNotFound = true);

        Candidate Get(int legacyCandidateId, bool errorIfNotFound = true);
        
        IEnumerable<Candidate> GetAllCandidatesWithPhoneNumber(string phoneNumber, bool errorIfNotFound = true);

        IEnumerable<Guid> GetCandidatesWithPendingMobileVerification();

        Candidate GetBySubscriberId(Guid subscriberId, bool errorIfNotFound = true);

        List<CandidateSummary> SearchCandidates(CandidateSearchRequest request);
    }

    public interface ICandidateWriteRepository : IWriteRepository<Candidate>
    {
    }

    public class CandidateSearchRequest
    {
        public CandidateSearchRequest(string firstName, string lastName, DateTime? dateOfBirth, string postcode)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Postcode = postcode;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public string Postcode { get; private set; }

        public override string ToString()
        {
            return $"CandidateSearchRequest FirstName: {FirstName}, LastName: {LastName}, DateOfBirth: {DateOfBirth}, Postcode: {Postcode}";
        }
    }
}