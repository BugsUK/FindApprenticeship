namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Audit;

    public interface IAuditReadRepository<T> : IReadRepository<AuditItem<T>> { }

    public interface IAuditWriteRepository<T> : IWriteRepository<AuditItem<T>> { }
}