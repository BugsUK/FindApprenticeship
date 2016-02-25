namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies.IoC
{
    using Domain.Raa.Interfaces.Repositories;
    using Mappers;
    using SFA.Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class VacancyRepositoryRegistry : Registry
    {
        public VacancyRepositoryRegistry()
        {
            // Apprenticeships.
            For<IMapper>().Use<Sql.Schemas.Vacancy.ApprenticeshipVacancyMappers>().Name = "ApprenticeshipVacancyMappers";

            For<IVacancyReadRepository>()
                .Use<Sql.Schemas.Vacancy.VacancyRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            For<IVacancyWriteRepository>()
                .Use<Sql.Schemas.Vacancy.VacancyRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            For<IVacancyLocationReadRepository>()
                .Use<VacancyLocationRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            For<IVacancyLocationWriteRepository>()
                .Use<VacancyLocationRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            For<IReferenceNumberRepository>()
                .Use<ReferenceNumberRepository>();

            // TODO: Traineeships.
        }
    }
}
