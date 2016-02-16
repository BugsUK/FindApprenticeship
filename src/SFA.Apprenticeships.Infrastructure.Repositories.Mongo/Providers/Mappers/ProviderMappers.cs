namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers.Mappers
{
    using Domain.Entities.Providers;
    using Infrastructure.Common.Mappers;
    using Entities;

    public class ProviderMappers : MapperEngine
    {
        public override void Initialise()
        {
            // TODO: SQL: AG: remove this class.
            // Mapper.CreateMap<Provider, MongoProvider>();
            // Mapper.CreateMap<MongoProvider, Provider>();

            Mapper.CreateMap<ProviderSite, MongoProviderSite>();
            Mapper.CreateMap<MongoProviderSite, ProviderSite>();

            Mapper.CreateMap<ProviderSiteEmployerLink, MongoProviderSiteEmployerLink>();
            Mapper.CreateMap<MongoProviderSiteEmployerLink, ProviderSiteEmployerLink>();
        }
    }
}
