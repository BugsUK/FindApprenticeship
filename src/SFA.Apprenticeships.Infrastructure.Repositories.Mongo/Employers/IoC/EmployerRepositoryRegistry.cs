namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Employers.IoC
{
    using Domain.Interfaces.Repositories;
    using Mappers;
    using SFA.Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class EmployerRepositoryRegistry : Registry
    {
        public EmployerRepositoryRegistry()
        {
            For<IMapper>().Use<EmployerMappers>().Name = "EmployerMappers";
            For<IEmployerReadRepository>().Use<EmployerRepository>().Ctor<IMapper>().Named("EmployerMappers");
            For<IEmployerWriteRepository>().Use<EmployerRepository>().Ctor<IMapper>().Named("EmployerMappers");
        }
    }
}