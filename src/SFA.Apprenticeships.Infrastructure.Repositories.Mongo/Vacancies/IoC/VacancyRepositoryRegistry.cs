namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies.IoC
{
    using Domain.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories;
    using Mappers;
    using SFA.Infrastructure.Interfaces;
    using Sql.Schemas.Vacancy;
    using StructureMap.Configuration.DSL;

    public class VacancyRepositoryRegistry : Registry
    {
        public VacancyRepositoryRegistry()
        {
            // Apprenticeships.
            For<IMapper>().Use<Sql.Schemas.Vacancy.VacancyMappers>().Name = "VacancyMappers";

            For<IVacancyReadRepository>()
                .Use<VacancyRepository>()
                .Ctor<IMapper>()
                .Named("VacancyMappers");

            For<IVacancyWriteRepository>()
                .Use<VacancyRepository>()
                .Ctor<IMapper>()
                .Named("VacancyMappers");

            For<IVacancyLocationReadRepository>()
                .Use<VacancyLocationRepository>()
                .Ctor<IMapper>()
                .Named("VacancyMappers");

            For<IVacancyLocationWriteRepository>()
                .Use<VacancyLocationRepository>()
                .Ctor<IMapper>()
                .Named("VacancyMappers");

            For<IReferenceNumberRepository>()
                .Use<ReferenceNumberRepository>();

            // TODO: Traineeships.
        }
    }
}
