namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Employers.Mappers
{
    using Domain.Entities.Raa.Parties;
    using Infrastructure.Common.Mappers;
    using Entities;

    public class EmployerMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Employer, MongoEmployer>();
            Mapper.CreateMap<MongoEmployer, Employer>();
        }
    }
}