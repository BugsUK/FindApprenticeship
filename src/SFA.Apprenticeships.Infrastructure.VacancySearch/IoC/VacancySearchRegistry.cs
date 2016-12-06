namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IoC
{
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Vacancies;
    using Application.Vacancy;
    using Configuration;
    using Domain.Entities.Vacancies;
    using Mappers;

    using SFA.Apprenticeships.Application.Interfaces;

    using StructureMap.Configuration.DSL;

    public class VacancySearchRegistry : Registry
    {
        public VacancySearchRegistry()
        {
            For<IMapper>().Singleton().Use<VacancySearchMapper>().Name = "VacancySearchMapper";

            For<IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>>().Use<ApprenticeshipsSearchProvider>().Ctor<IMapper>().Named("VacancySearchMapper");
            For<IVacancySearchProvider<TraineeshipSearchResponse, TraineeshipSearchParameters>>().Use<TraineeshipsSearchProvider>().Ctor<IMapper>().Named("VacancySearchMapper");

            For<IAllApprenticeshipVacanciesProvider>().Use<AllApprenticeshipVacanciesProvider>().Ctor<IMapper>().Named("VacancySearchMapper");
            For<IAllTraineeshipVacanciesProvider>().Use<AllTraineeshipVacanciesProvider>().Ctor<IMapper>().Named("VacancySearchMapper");
        }
    }
}
