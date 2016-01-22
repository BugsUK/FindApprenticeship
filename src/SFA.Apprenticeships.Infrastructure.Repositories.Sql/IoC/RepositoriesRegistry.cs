namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IoC
{
    using Common;
    using Configuration;
    using Domain.Interfaces.Repositories;
    using Schemas.Reference;
    using SFA.Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class RepositoriesRegistry : Registry
    {
        public RepositoriesRegistry(SqlConfiguration configuration)
        {
            //Common
            For<IGetOpenConnection>().Use<GetOpenConnectionFromConnectionString>().Ctor<string>("connectionString").Is(configuration.ConnectionString);

            //Mappers
            For<IMapper>().Use<ReferenceMappers>().Name = "ReferenceMappers";

            //Repositories
            For<IReferenceRepository>().Use<SqlReferenceRepository>().Ctor<IMapper>().Named("ReferenceMappers");
        }
    }
}