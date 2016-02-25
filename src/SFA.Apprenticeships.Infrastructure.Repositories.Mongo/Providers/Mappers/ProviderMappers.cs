namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers.Mappers
{
    using Domain.Entities.Raa.Parties;
    using Infrastructure.Common.Mappers;
    using Entities;

    public class ProviderMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Provider, MongoProvider>();
            Mapper.CreateMap<MongoProvider, Provider>();

            Mapper.CreateMap<ProviderSite, MongoProviderSite>();
            Mapper.CreateMap<MongoProviderSite, ProviderSite>();

            Mapper.CreateMap<VacancyParty, MongoVacancyParty>();
            Mapper.CreateMap<MongoVacancyParty, VacancyParty>();
        }
    }
}