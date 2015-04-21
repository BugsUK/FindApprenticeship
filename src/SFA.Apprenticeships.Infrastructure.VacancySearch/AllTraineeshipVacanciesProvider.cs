namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using Application.Interfaces.Logging;
    using Application.Interfaces.Vacancies;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;

    public class AllTraineeshipVacanciesProvider : AllVacanciesProviderBase<TraineeshipSearchResponse, TraineeshipSummary>
    {
        public AllTraineeshipVacanciesProvider(
            ILogService logger,
            IElasticsearchClientFactory elasticsearchClientFactory,
            IMapper vacancySearchMapper)
            : base(logger, elasticsearchClientFactory, vacancySearchMapper)
        {
        }
    }
}