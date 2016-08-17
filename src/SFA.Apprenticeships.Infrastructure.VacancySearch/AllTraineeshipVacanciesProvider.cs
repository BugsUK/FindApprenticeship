namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using SFA.Infrastructure.Interfaces;
    using Application.Vacancy;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;

    using SFA.Apprenticeships.Application.Interfaces;

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