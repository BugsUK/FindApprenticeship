namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IoC
{
    using Common;
    using Configuration;
    using Domain.Interfaces.Repositories;
    using Schemas.dbo;
    using Schemas.Provider;
    using Schemas.Reference;
    using Schemas.UserProfile;
    using SFA.Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class RepositoriesRegistry : Registry
    {
        public RepositoriesRegistry(SqlConfiguration configuration)
        {
            // Common.
            For<IGetOpenConnection>().Use<GetOpenConnectionFromConnectionString>().Ctor<string>("connectionString").Is(configuration.ConnectionString);

            // Mappers.
            For<IMapper>().Use<ReferenceMappers>().Name = "ReferenceMappers";
            For<IMapper>().Use<AgencyUserMappers>().Name = "AgencyUserMappers";
            For<IMapper>().Use<ProviderUserMappers>().Name = "ProviderUserMappers";
            For<IMapper>().Use<VacancyOwnerRelationshipMappers>().Name = "VacancyOwnerRelationshipMappers";
            For<IMapper>().Use<EmployerMappers>().Name = "EmployerMappers";
            For<IMapper>().Use<ProviderSiteMappers>().Name = "ProviderSiteMappers";

            
            // Repositories.
            For<IReferenceRepository>().Use<ReferenceRepository>().Ctor<IMapper>().Named("ReferenceMappers");

            For<IAgencyUserReadRepository>().Use<AgencyUserRepository>().Ctor<IMapper>().Named("AgencyUserMappers");
            For<IAgencyUserWriteRepository>().Use<AgencyUserRepository>().Ctor<IMapper>().Named("AgencyUserMappers");

            For<IProviderUserReadRepository>().Use<ProviderUserRepository>().Ctor<IMapper>().Named("ProviderUserMappers");
            For<IProviderUserWriteRepository>().Use<ProviderUserRepository>().Ctor<IMapper>().Named("ProviderUserMappers");

            For<IProviderSiteEmployerLinkReadRepository>().Use<VacancyOwnerRelationshipRepository>().Ctor<IMapper>().Named("VacancyOwnerRelationshipMappers");
            For<IProviderSiteEmployerLinkWriteRepository>().Use<VacancyOwnerRelationshipRepository>().Ctor<IMapper>().Named("VacancyOwnerRelationshipMappers");

            For<IProviderSiteReadRepository>().Use<ProviderSiteRepository>().Ctor<IMapper>().Named("ProviderSiteMappers");
            For<IProviderSiteWriteRepository>().Use<ProviderSiteRepository>().Ctor<IMapper>().Named("ProviderSiteMappers");

            For<IEmployerReadRepository>().Use<EmployerRepository>().Ctor<IMapper>().Named("EmployerMappers");
            For<IEmployerWriteRepository>().Use<EmployerRepository>().Ctor<IMapper>().Named("EmployerMappers");
        }
    }
}
