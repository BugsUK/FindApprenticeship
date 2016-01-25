namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.IoC
{
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using Mappers;
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
    }
}
