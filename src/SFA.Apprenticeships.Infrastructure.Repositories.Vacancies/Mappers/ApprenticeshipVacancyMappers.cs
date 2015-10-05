namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Entities;

    public class ApprenticeshipVacancyMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipVacancy, MongoApprenticeshipVacancy>();
            Mapper.CreateMap<MongoApprenticeshipVacancy, ApprenticeshipVacancy>();
        }
    }
}
