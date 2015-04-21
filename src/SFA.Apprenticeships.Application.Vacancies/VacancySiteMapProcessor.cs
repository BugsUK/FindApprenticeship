namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Caching;
    using Interfaces.Logging;
    using Interfaces.Vacancies;
    using Vacancy;
    using Web.Common.SiteMap;

    // TODO: AG: US438: logging.
    public class VacancySiteMapProcessor : IVacancySiteMapProcessor
    {
        private readonly ILogService _logger;
        private readonly ICacheService _cacheService;
        private readonly IAllVacanciesProvider<ApprenticeshipSearchResponse> _apprenticeshipVacanciesProvider;
        private readonly IAllVacanciesProvider<TraineeshipSearchResponse> _traineeshipVacanciesProvider;

        public VacancySiteMapProcessor(
            ILogService logger,
            ICacheService cacheService,
            IAllVacanciesProvider<ApprenticeshipSearchResponse> apprenticeshipVacanciesProvider,
            IAllVacanciesProvider<TraineeshipSearchResponse> traineeshipVacanciesProvider)
        {
            _logger = logger;
            _cacheService = cacheService;
            _apprenticeshipVacanciesProvider = apprenticeshipVacanciesProvider;
            _traineeshipVacanciesProvider = traineeshipVacanciesProvider;
        }

        public void CreateVacancySiteMap(CreateVacancySiteMapRequest request)
        {
            var apprenticeshipVacancies = _apprenticeshipVacanciesProvider.GetAllVacancies();
            var traineeshipVacancies = _traineeshipVacanciesProvider.GetAllVacancies();

            var siteMapItems = new List<SiteMapItem>();

            const SiteMapChangeFrequency changeFrequency = SiteMapChangeFrequency.Hourly;
            var lastModified = DateTime.Today;

            siteMapItems.AddRange(
                apprenticeshipVacancies
                    .Select(
                        vacancy => new SiteMapItem(CreateVacancySiteMapUrl(vacancy), lastModified, changeFrequency))
                    .Union(
                        traineeshipVacancies.Select(
                            vacancy => new SiteMapItem(CreateVacancySiteMapUrl(vacancy), lastModified, changeFrequency))));

            _cacheService.PutObject("SiteMap.Vacancies", siteMapItems.ToArray());
        }

        private static string CreateVacancySiteMapUrl(ApprenticeshipSearchResponse vacancy)
        {
            // TODO: AG: US438: consider putting into configuration.
            return string.Format("https://www.findapprenticeship.service.gov.uk/apprenticeship/{0}", vacancy.Id);
        }

        private static string CreateVacancySiteMapUrl(TraineeshipSearchResponse vacancy)
        {
            // TODO: AG: US438: consider putting into configuration.
            return string.Format("https://www.findapprenticeship.service.gov.uk/traineeship/{0}", vacancy.Id);
        }
    }
}