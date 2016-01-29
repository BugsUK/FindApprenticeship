namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies.IoC
{
    using Domain.Interfaces.Repositories;
    using Mappers;
    using Sql.Configuration;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using StructureMap.Configuration.DSL;

    public class VacancyRepositoryRegistry : Registry
    {
        public VacancyRepositoryRegistry()
        {
            // Apprenticeships.
            For<IMapper>().Use<ApprenticeshipVacancyMappers>().Name = "ApprenticeshipVacancyMappers";

            For<IApprenticeshipVacancyReadRepository>()
                .Use<ApprenticeshipVacancyRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            For<IApprenticeshipVacancyWriteRepository>()
                .Use<ApprenticeshipVacancyRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            For<IOfflineApprenticeshipVacancyRepository>()
                .Use<OfflineApprenticeshipVacancyRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            For<IReferenceNumberRepository>()
                .Use<ReferenceNumberRepository>();

            // TODO: Traineeships.
        }

        public VacancyRepositoryRegistry(SqlConfiguration sqlConfiguration)
        {
            //Common
            For<IGetOpenConnection>().Use<GetOpenConnectionFromConnectionString>().Ctor<string>("connectionString").Is(sqlConfiguration.ConnectionString);

            // Apprenticeships.
            For<IMapper>().Use<Sql.Schemas.Vacancy.ApprenticeshipVacancyMappers>().Name = "ApprenticeshipVacancyMappers";

            For<IApprenticeshipVacancyReadRepository>()
                .Use<Sql.Schemas.Vacancy.ApprenticeshipVacancyRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            For<IApprenticeshipVacancyWriteRepository>()
                .Use<Sql.Schemas.Vacancy.ApprenticeshipVacancyRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            For<IOfflineApprenticeshipVacancyRepository>()
                .Use<OfflineApprenticeshipVacancyRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            For<IReferenceNumberRepository>()
                .Use<ReferenceNumberRepository>();
        }
    }
}
