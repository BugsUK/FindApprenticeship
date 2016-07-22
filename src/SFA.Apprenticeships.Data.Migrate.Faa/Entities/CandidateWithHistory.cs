namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities
{
    using System.Collections.Generic;
    using Sql;

    public class CandidateWithHistory
    {
        public CandidatePerson CandidatePerson { get; set; }
        public IList<CandidateHistory> CandidateHistory { get; set; }
    }
}