namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Application.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Application.Vacancy;

    public class CheckNasGateway : IMonitorTask
    {
        private readonly ILogService _logger;
        private readonly IVacancyDataProvider<ApprenticeshipVacancyDetail> _vacancyDataProvider;
        private readonly IVacancyIndexDataProvider _vacancyIndexDataProvider;

        public CheckNasGateway(IVacancyIndexDataProvider vacancyIndexDataProvider,
            IVacancyDataProvider<ApprenticeshipVacancyDetail> vacancyDataProvider, ILogService logger)
        {
            _vacancyIndexDataProvider = vacancyIndexDataProvider;
            _vacancyDataProvider = vacancyDataProvider;
            _logger = logger;
        }

        public string TaskName
        {
            get { return "Check NAS gateway"; }
        }

        public void Run()
        {
            var summaries = _vacancyIndexDataProvider.GetVacancySummaries(1);

            var apprenticeshipSummaries = summaries as IList<ApprenticeshipSummary> ?? summaries.ApprenticeshipSummaries.ToList();
            var traineeshipSummaries = summaries as IList<TraineeshipSummary> ?? summaries.TraineeshipSummaries;

            var summary = apprenticeshipSummaries.ToList().FirstOrDefault();

            if (summary != null)
            {
                _vacancyDataProvider.GetVacancyDetails(summary.Id);
            }
            else
            {
                _logger.Error("Monitor get vacancy summary returned {0} records", apprenticeshipSummaries.Count() + traineeshipSummaries.Count());
            }
        }
    }
}
