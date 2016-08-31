namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using SFA.Infrastructure.Interfaces;
    using Application.Vacancy;
    using Elastic.Common.Configuration;

    using SFA.Apprenticeships.Application.Interfaces;

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
