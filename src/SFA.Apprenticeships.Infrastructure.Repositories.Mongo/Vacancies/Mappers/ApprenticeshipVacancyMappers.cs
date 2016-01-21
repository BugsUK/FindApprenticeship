namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies.Mappers
{
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Infrastructure.Common.Mappers;
    using Mongo.Vacancies.Entities;

    public class ApprenticeshipVacancyMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipVacancy, MongoApprenticeshipVacancy>();
            Mapper.CreateMap<MongoApprenticeshipVacancy, ApprenticeshipVacancy>();
        }
    }
}
