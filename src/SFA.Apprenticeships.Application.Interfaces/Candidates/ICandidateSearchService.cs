namespace SFA.Apprenticeships.Application.Interfaces.Candidates
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;

    public interface ICandidateSearchService
    {
        List<CandidateSummary> SearchCandidates(CandidateSearchRequest request);
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
    }
}