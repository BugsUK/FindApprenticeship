namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers.IoC
{
    using Domain.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories;
    using Mappers;
    using SFA.Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class ProviderRepositoryRegistry : Registry
    {
        public ProviderRepositoryRegistry()
        {
            For<IMapper>().Use<ProviderMappers>().Name = "ProviderMappers";
            For<IProviderReadRepository>().Use<ProviderRepository>().Ctor<IMapper>().Named("ProviderMappers");
            For<IProviderWriteRepository>().Use<ProviderRepository>().Ctor<IMapper>().Named("ProviderMappers");
            For<IProviderSiteReadRepository>().Use<ProviderSiteRepository>().Ctor<IMapper>().Named("ProviderMappers");
            For<IProviderSiteWriteRepository>().Use<ProviderSiteRepository>().Ctor<IMapper>().Named("ProviderMappers");
            For<IVacancyPartyReadRepository>().Use<VacancyPartyRepository>().Ctor<IMapper>().Named("ProviderMappers");
            For<IVacancyPartyWriteRepository>().Use<VacancyPartyRepository>().Ctor<IMapper>().Named("ProviderMappers");
        }
    }
}