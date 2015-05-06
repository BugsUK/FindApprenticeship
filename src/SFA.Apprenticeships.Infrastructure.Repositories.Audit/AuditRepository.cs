namespace SFA.Apprenticeships.Infrastructure.Repositories.Audit
{
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;

    public class AuditRepository : IAuditRepository
    {
        public void Audit(User user, string eventType)
        {
            //todo: create an AuditItem and write
            throw new System.NotImplementedException();
        }
    }
}