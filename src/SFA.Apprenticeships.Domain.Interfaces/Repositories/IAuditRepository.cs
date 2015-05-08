namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Candidates;
    using Entities.Users;

    public interface IAuditRepository
    {
        void Audit(User user, string eventType);
        void Audit(Candidate candidate, string eventType);
    }
}