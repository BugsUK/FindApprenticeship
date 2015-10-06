namespace SFA.Apprenticeships.Infrastructure.Repositories.Employers.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Organisations;
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