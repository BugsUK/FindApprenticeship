namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using Application.Interfaces.Logging;
    using Application.Vacancy;
    using Elastic.Common.Configuration;
    using ApprenticeshipSummary = Elastic.Common.Entities.ApprenticeshipSummary;

    public class AllApprenticeshipVacanciesProvider : AllVacanciesProviderBase<ApprenticeshipSummary>, IAllApprenticeshipVacanciesProvider
    {
        public AllApprenticeshipVacanciesProvider(
            ILogService logger,
            IElasticsearchClientFactory elasticsearchClientFactory)
            : base(logger, elasticsearchClientFactory)
        {
        }
    }
}
