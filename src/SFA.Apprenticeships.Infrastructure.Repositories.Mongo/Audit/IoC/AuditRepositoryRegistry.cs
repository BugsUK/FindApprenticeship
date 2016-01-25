namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Audit.IoC
{
    using Domain.Interfaces.Repositories;
    using StructureMap.Configuration.DSL;

    public class AuditRepositoryRegistry : Registry
    {
        public AuditRepositoryRegistry()
        {
            For<IAuditRepository>().Use<AuditRepository>();
        }
    }
}
