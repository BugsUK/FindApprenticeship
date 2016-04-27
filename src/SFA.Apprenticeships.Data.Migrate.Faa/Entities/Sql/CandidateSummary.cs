namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    using System;

    public class CandidateSummary
    {
        public int CandidateId { get; set; }
        public int PersonId { get; set; }
        public Guid CandidateGuid { get; set; }
    }
}