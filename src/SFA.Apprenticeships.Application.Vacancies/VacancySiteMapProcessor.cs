namespace SFA.Apprenticeships.Application.Vacancies
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Vacancies;
    using Interfaces.Logging;
    using Vacancy;
    using Web.Common.SiteMap;

    // TODO: AG: US438: logging.
    public class VacancySiteMapProcessor : IVacancySiteMapProcessor
    {
        private readonly ILogService _logger;
        private readonly ISiteMapVacancyProvider _siteMapVacancyProvider;
        private readonly IAllApprenticeshipVacanciesProvider _apprenticeshipVacanciesProvider;
        private readonly IAllTraineeshipVacanciesProvider _traineeshipVacanciesProvider;

        public VacancySiteMapProcessor(
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

            siteMapVacancies.AddRange(
                apprenticeshipVacancies.Select(vacancyId =>
                    new SiteMapVacancy
                    {
                        VacancyId = vacancyId,
                        VacancyType = VacancyType.Apprenticeship
                    })
                    .Union(
                        traineeshipVacancies.Select(vacancyId =>
                            new SiteMapVacancy
                            {
                                VacancyId = vacancyId,
                                VacancyType = VacancyType.Traineeship
                            })));

            _logger.Info("Caching {0} apprenticeship + {1} traineeship vacancies",
                apprenticeshipVacancies.Count(), traineeshipVacancies.Count());

            _siteMapVacancyProvider.SetVacancies(siteMapVacancies);

            _logger.Info("Created site map based on apprenticeship and traineeship indexes '{0}' and '{1}' respectively",
                request.ApprenticeshipVacancyIndexName, request.TraineeshipVacancyIndexName);
        }
    }
}