namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers.IoC
{
    using Domain.Raa.Interfaces.Repositories;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using Sql.Schemas.Provider;
    using Sql.Schemas.dbo;
    using StructureMap.Configuration.DSL;

    public class ProviderRepositoryRegistry : Registry
    {
        public ProviderRepositoryRegistry()
        {
            For<IMapper>().Singleton().Use<ProviderMappers>().Name = "ProviderMappers";
            For<IMapper>().Singleton().Use<ProviderSiteMappers>().Name = "ProviderSiteMappers";
            For<IMapper>().Singleton().Use<VacancyOwnerRelationshipMappers>().Name = "VacancyOwnerRelationshipMappers";

            For<IProviderReadRepository>().Use<ProviderRepository>().Ctor<IMapper>().Named("ProviderMappers");
            For<IProviderWriteRepository>().Use<ProviderRepository>().Ctor<IMapper>().Named("ProviderMappers");

            For<IProviderSiteReadRepository>().Use<ProviderSiteRepository>().Ctor<IMapper>().Named("ProviderSiteMappers");
            For<IProviderSiteWriteRepository>().Use<ProviderSiteRepository>().Ctor<IMapper>().Named("ProviderSiteMappers");

            For<IVacancyOwnerRelationshipReadRepository>().Use<VacancyOwnerRelationshipRepository>().Ctor<IMapper>().Named("VacancyOwnerRelationshipMappers");
            For<IVacancyOwnerRelationshipWriteRepository>().Use<VacancyOwnerRelationshipRepository>().Ctor<IMapper>().Named("VacancyOwnerRelationshipMappers");
        }
    }
}