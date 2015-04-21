namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Interfaces.Caching;
    using Interfaces.Logging;
    using Vacancy;
    using Web.Common.SiteMap;

    // TODO: AG: US438: logging.
    public class VacancySiteMapProcessor : IVacancySiteMapProcessor
    {
        private readonly ILogService _logger;
        private readonly ICacheService _cacheService;
        private readonly IAllApprenticeshipVacanciesProvider _apprenticeshipVacanciesProvider;
        private readonly IAllTraineeshipVacanciesProvider _traineeshipVacanciesProvider;

        public VacancySiteMapProcessor(
            ILogService logger,
            ICacheService cacheService,
            IAllApprenticeshipVacanciesProvider apprenticeshipVacanciesProvider,
            IAllTraineeshipVacanciesProvider traineeshipVacanciesProvider)
        {
            _logger = logger;
            _cacheService = cacheService;
            _apprenticeshipVacanciesProvider = apprenticeshipVacanciesProvider;
            _traineeshipVacanciesProvider = traineeshipVacanciesProvider;
        }

        public void CreateVacancySiteMap(CreateVacancySiteMapRequest request)
        {
            _logger.Info("Creating site map based on apprenticeship and traineeship indexes '{0}' and '{1}' respectively",
                request.ApprenticeshipVacancyIndexName, request.TraineeshipVacancyIndexName);

            var apprenticeshipVacancies = _apprenticeshipVacanciesProvider.GetAllVacancyIds(request.ApprenticeshipVacancyIndexName).ToList();
            var traineeshipVacancies = _traineeshipVacanciesProvider.GetAllVacancyIds(request.TraineeshipVacancyIndexName).ToList();

            var siteMapItems = new List<SiteMapItem>();

            const SiteMapChangeFrequency changeFrequency = SiteMapChangeFrequency.Hourly;
            var lastModified = DateTime.Today;

            siteMapItems.AddRange(
                apprenticeshipVacancies
                    .Select(
                        vacancyId => new SiteMapItem(CreateApprenticeshipVacancySiteMapUrl(vacancyId), lastModified, changeFrequency))
                    .Union(
                        traineeshipVacancies.Select(
                            vacancyId => new SiteMapItem(CreateTraineeshipVacancySiteMapUrl(vacancyId), lastModified, changeFrequency))));

            _logger.Info("Caching {0} apprenticeship + {1} traineeship vacancies",
                apprenticeshipVacancies.Count(), traineeshipVacancies.Count());

            // TODO: AG: US438: where to put cach keys?
            // TODO: AG: US438: review cache duration.
            _cacheService.PutObject("SiteMap.Vacancies", siteMapItems.ToArray(), CacheDuration.OneDay);

            _logger.Info("Created site map based on apprenticeship and traineeship indexes '{0}' and '{1}' respectively",
                request.ApprenticeshipVacancyIndexName, request.TraineeshipVacancyIndexName);
        }

        private static string CreateApprenticeshipVacancySiteMapUrl(int vacancyId)
        {
            // TODO: AG: US438: consider putting into configuration.
            return string.Format("https://www.findapprenticeship.service.gov.uk/apprenticeship/{0}", vacancyId);
        }

        private static string CreateTraineeshipVacancySiteMapUrl(int vacancyId)
        {
            // TODO: AG: US438: consider putting into configuration.
            return string.Format("https://www.findapprenticeship.service.gov.uk/traineeship/{0}", vacancyId);
        }
    }
}