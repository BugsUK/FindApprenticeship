namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider.IoC
{
    using Application.Interfaces;
    using Domain.Raa.Interfaces.Repositories;
    using dbo;
    using Provider;
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