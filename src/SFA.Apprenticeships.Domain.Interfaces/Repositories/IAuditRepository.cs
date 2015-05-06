namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Users;

    public interface IAuditRepository
    {
        void Audit(User user, string eventType);
    }
}