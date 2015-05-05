namespace SFA.Apprenticeships.Infrastructure.Repositories.Audit.IoC
{
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using StructureMap.Configuration.DSL;

    public class AuditRepositoryRegistry : Registry
    {
        public AuditRepositoryRegistry()
        {
            For<IAuditReadRepository<User>>().Use<AuditRepository<User>>();
            For<IAuditWriteRepository<User>>().Use<AuditRepository<User>>();
        }
    }
}
