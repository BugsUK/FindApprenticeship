namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IoC
{
    using Common;
    using Configuration;
    using Domain.Interfaces.Repositories;
    using Schemas.Reference;
    using Schemas.WebService;
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
            For<IMapper>().Use<WebServiceMappers>().Name = "WebServiceMappers";

            //Repositories
            For<IReferenceRepository>().Use<ReferenceRepository>().Ctor<IMapper>().Named("ReferenceMappers");
            // For<IWebServiceRepository>().Use<SqlWebServerRepository>().Ctor<IMapper>().Named("WebServiceMappers");
        }
    }
}