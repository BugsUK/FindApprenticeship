namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies.Mappers
{
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Infrastructure.Common.Mappers;
    using Entities;

    public class VacancyMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Vacancy, MongoVacancy>();
            Mapper.CreateMap<MongoVacancy, Vacancy>();

            Mapper.CreateMap<VacancyLocation, MongoVacancyLocation>();
            Mapper.CreateMap<MongoVacancyLocation, VacancyLocation>();
        }
    }
}
