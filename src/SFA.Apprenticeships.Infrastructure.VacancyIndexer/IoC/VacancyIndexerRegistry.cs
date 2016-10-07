namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IoC
{
    using SFA.Infrastructure.Interfaces;
    using Application.Vacancies.Entities;
    using Elastic.Common.Entities;
    using Mappers;

    using SFA.Apprenticeships.Application.Interfaces;

    using StructureMap.Configuration.DSL;

    public class VacancyIndexerRegistry : Registry
    {
        public VacancyIndexerRegistry()
        {
            For<IMapper>().Singleton().Use<VacancyIndexerMapper>().Name = "VacancyIndexerMapper";

            For<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>>()
                .Singleton()
                .Use<VacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>>()
                .Ctor<IMapper>()
                .Named("VacancyIndexerMapper");

            For<IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary>>()
                .Singleton()
                .Use<VacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary>>()
                .Ctor<IMapper>()
                .Named("VacancyIndexerMapper");

        }
    }
}
