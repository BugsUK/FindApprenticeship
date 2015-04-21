namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using Application.Interfaces.Logging;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Configuration;
    using ApprenticeshipSummary = Elastic.Common.Entities.ApprenticeshipSummary;

    public class AllApprenticeshipVacanciesProvider : AllVacanciesProviderBase<ApprenticeshipSearchResponse, ApprenticeshipSummary>
    {
        public AllApprenticeshipVacanciesProvider(
            ILogService logger,
            IElasticsearchClientFactory elasticsearchClientFactory,
            IMapper vacancySearchMapper)
            : base(logger, elasticsearchClientFactory, vacancySearchMapper)
        {
        }
    }
}
