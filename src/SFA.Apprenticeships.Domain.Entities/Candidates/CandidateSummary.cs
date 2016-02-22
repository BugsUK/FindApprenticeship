namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;
    using Locations;

    public class CandidateSummary : BaseEntity
    {
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
    }
}