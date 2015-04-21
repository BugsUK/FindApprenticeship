namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using Application.Interfaces.Logging;
    using Application.Vacancy;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;

    public class AllTraineeshipVacanciesProvider : AllVacanciesProviderBase<TraineeshipSummary>, IAllTraineeshipVacanciesProvider
    {
        public AllTraineeshipVacanciesProvider(
            ILogService logger,
            IElasticsearchClientFactory elasticsearchClientFactory)
            : base(logger, elasticsearchClientFactory)
        {
        }
    }
}