namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities
{
    using Sql;

    public class CandidatePerson
    {
        public Candidate Candidate { get; set; }
        public Person Person { get; set; } 
    }
}