namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Employer.IoC
{
    using Application.Interfaces;
    using Domain.Raa.Interfaces.Repositories;
    using dbo;
    using StructureMap.Configuration.DSL;

    public class EmployerRepositoryRegistry : Registry
    {
        public EmployerRepositoryRegistry()
        {
            For<IMapper>().Singleton().Use<EmployerMappers>().Name = "EmployerMappers";
            For<IEmployerReadRepository>().Use<EmployerRepository>().Ctor<IMapper>().Named("EmployerMappers");
            For<IEmployerWriteRepository>().Use<EmployerRepository>().Ctor<IMapper>().Named("EmployerMappers");
        }
    }
}