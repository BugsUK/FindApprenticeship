namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.IoC
{
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class VacancyRepositoryRegistry : Registry
    {
        public VacancyRepositoryRegistry()
        {
            // Apprenticeships.
            For<IMapper>().Use<ApprenticeshipVacancyMappers>().Name = "ApprenticeshipVacancyMappers";

            For<IApprenticeshipVacancyWriteRepository>()
                .Use<ApprenticeshipVacancyRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            For<IApprenticeshipVacancyReadRepository>()
                .Use<ApprenticeshipVacancyRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            // TODO: Traineeships.
        }
    }
}
