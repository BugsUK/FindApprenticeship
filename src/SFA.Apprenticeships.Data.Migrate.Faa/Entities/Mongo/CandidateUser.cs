namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Mongo
{
    public class CandidateUser
    {
        public User User { get; set; }
        public Candidate Candidate { get; set; }
    }
}