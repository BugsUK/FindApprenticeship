namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers.IoC
{
    using Domain.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using Sql.Schemas.Provider;
    using StructureMap.Configuration.DSL;
    
    public class ProviderRepositoryRegistry : Registry
    {
        public ProviderRepositoryRegistry()
        {
            For<IMapper>().Use<ProviderMappers>().Name = "ProviderMappers";
            For<IMapper>().Use<Mappers.ProviderMappers>().Name = "MongoProviderMappers";

            For<IProviderReadRepository>().Use<ProviderRepository>().Ctor<IMapper>().Named("ProviderMappers");
            For<IProviderWriteRepository>().Use<ProviderRepository>().Ctor<IMapper>().Named("ProviderMappers");

            For<IProviderSiteReadRepository>().Use<ProviderSiteRepository>().Ctor<IMapper>().Named("MongoProviderMappers");
            For<IProviderSiteWriteRepository>().Use<ProviderSiteRepository>().Ctor<IMapper>().Named("MongoProviderMappers");

            For<IProviderSiteEmployerLinkReadRepository>().Use<ProviderSiteEmployerLinkRepository>().Ctor<IMapper>().Named("MongoProviderMappers");
            For<IProviderSiteEmployerLinkWriteRepository>().Use<ProviderSiteEmployerLinkRepository>().Ctor<IMapper>().Named("MongoProviderMappers");
        }
    }
}
