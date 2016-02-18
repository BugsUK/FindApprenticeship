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
            For<IMapper>().Use<ApprenticeshipVacancyMappers>().Name = "ApprenticeshipVacancyMappers";

            For<IVacancyReadRepository>()
                .Use<VacancyRepository>()
                .Ctor<IMapper>()
                .Named("ApprenticeshipVacancyMappers");

            For<IVacancyWriteRepository>()
                .Use<VacancyRepository>()
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
