namespace SFA.Apprenticeships.Infrastructure.Repositories.Providers.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Providers;
    using Entities;

    public class ProviderMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Provider, MongoProvider>();
            Mapper.CreateMap<MongoProvider, Provider>();

            Mapper.CreateMap<ProviderSite, MongoProviderSite>();
            Mapper.CreateMap<MongoProviderSite, ProviderSite>();

            Mapper.CreateMap<ProviderSiteEmployerLink, MongoProviderSiteEmployerLink>();
            Mapper.CreateMap<MongoProviderSiteEmployerLink, ProviderSiteEmployerLink>();
        }
    }
}