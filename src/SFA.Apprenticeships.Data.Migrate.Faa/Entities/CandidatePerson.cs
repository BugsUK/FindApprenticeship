namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities
{
    using Repository.Sql;
    using Sql;

    public class CandidatePerson
    {
        public Candidate Candidate { get; set; }
        public Person Person { get; set; }
        public SchoolAttended SchoolAttended { get; set; }
    }
}