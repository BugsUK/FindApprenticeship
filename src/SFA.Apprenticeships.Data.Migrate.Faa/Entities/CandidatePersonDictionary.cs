namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities
{
    using System.Collections.Generic;
    using Sql;

    public class CandidatePersonDictionary
    {
        public IDictionary<string, object> Candidate { get; set; }
        public IDictionary<string, object> Person { get; set; } 
    }
}