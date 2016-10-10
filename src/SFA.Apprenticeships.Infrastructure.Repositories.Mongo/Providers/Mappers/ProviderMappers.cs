namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers.Mappers
{
    using Domain.Entities.Raa.Parties;
    using Infrastructure.Common.Mappers;
    using Entities;

    public class ProviderMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<VacancyOwnerRelationship, MongoVacancyOwnerRelationship>();
            Mapper.CreateMap<MongoVacancyOwnerRelationship, VacancyOwnerRelationship>();
        }
    }
}
