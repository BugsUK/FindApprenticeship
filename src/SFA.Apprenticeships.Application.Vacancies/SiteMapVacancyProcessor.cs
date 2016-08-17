namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Vacancies;
    using Entities.SiteMap;

    using SFA.Apprenticeships.Application.Interfaces;
    using Vacancy;
    using Vacancy.SiteMap;

    public class SiteMapVacancyProcessor : ISiteMapVacancyProcessor
    {
        private readonly ILogService _logger;
        private readonly ISiteMapVacancyProvider _siteMapVacancyProvider;
        private readonly IAllApprenticeshipVacanciesProvider _apprenticeshipVacanciesProvider;
        private readonly IAllTraineeshipVacanciesProvider _traineeshipVacanciesProvider;

        public SiteMapVacancyProcessor(
            ILogService logger,
            ISiteMapVacancyProvider siteMapVacancyProvider,
            IAllApprenticeshipVacanciesProvider apprenticeshipVacanciesProvider,
            IAllTraineeshipVacanciesProvider traineeshipVacanciesProvider)
        {
            _logger = logger;
            _siteMapVacancyProvider = siteMapVacancyProvider;
            _apprenticeshipVacanciesProvider = apprenticeshipVacanciesProvider;
            _traineeshipVacanciesProvider = traineeshipVacanciesProvider;
        }

        public void Process(CreateVacancySiteMapRequest request)
        {
            _logger.Info("Creating site map based on apprenticeship and traineeship indexes '{0}' and '{1}' respectively",
                request.ApprenticeshipVacancyIndexName, request.TraineeshipVacancyIndexName);

            var apprenticeshipVacancies = _apprenticeshipVacanciesProvider.GetAllVacancyIds(request.ApprenticeshipVacancyIndexName).ToList();
            var traineeshipVacancies = _traineeshipVacanciesProvider.GetAllVacancyIds(request.TraineeshipVacancyIndexName).ToList();

            var siteMapVacancies = new List<SiteMapVacancy>();
            var lastModifiedDate = DateTime.UtcNow;

            siteMapVacancies.AddRange(
                apprenticeshipVacancies.Select(vacancyId =>
                    new SiteMapVacancy
                    {
                        VacancyId = vacancyId,
                        VacancyType = VacancyType.Apprenticeship,
                        LastModifiedDate = lastModifiedDate
                    })
                    .Union(
                        traineeshipVacancies.Select(vacancyId =>
                            new SiteMapVacancy
                            {
                                VacancyId = vacancyId,
                                VacancyType = VacancyType.Traineeship,
                                LastModifiedDate = lastModifiedDate
                            })));

            _logger.Info("Caching {0} apprenticeship + {1} traineeship vacancies",
                apprenticeshipVacancies.Count(), traineeshipVacancies.Count());

            _siteMapVacancyProvider.SetVacancies(siteMapVacancies);

            _logger.Info("Created site map based on apprenticeship and traineeship indexes '{0}' and '{1}' respectively",
                request.ApprenticeshipVacancyIndexName, request.TraineeshipVacancyIndexName);
        }
    }
}