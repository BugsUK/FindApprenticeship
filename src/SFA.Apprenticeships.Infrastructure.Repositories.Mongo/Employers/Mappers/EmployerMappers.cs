namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Employers.Mappers
{
    using Domain.Entities.Organisations;
    using Infrastructure.Common.Mappers;
    using Mongo.Employers.Entities;

    public class EmployerMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Employer, MongoEmployer>();
            Mapper.CreateMap<MongoEmployer, Employer>();
        }
    }
}