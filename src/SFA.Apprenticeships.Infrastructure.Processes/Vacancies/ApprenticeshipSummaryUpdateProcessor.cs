namespace SFA.Apprenticeships.Infrastructure.Processes.Vacancies
{
    using System;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.Interfaces.ReferenceData;
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Domain.Interfaces.Configuration;
    using Configuration;
    using Domain.Interfaces.Messaging;
    using VacancyIndexer;
    using Elastic = Elastic.Common.Entities;

    public class ApprenticeshipSummaryUpdateProcessor : IApprenticeshipSummaryUpdateProcessor
    {
        private readonly IReferenceDataService _referenceDataService;
        private readonly IMessageBus _messageBus;
        private readonly ILogService _logService;
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, Elastic.ApprenticeshipSummary> _vacancyIndexerService;
        private readonly int _vacancyAboutToExpireThreshold;

        public ApprenticeshipSummaryUpdateProcessor(
            IConfigurationService configurationService,
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, Elastic.ApprenticeshipSummary> vacancyIndexerService,
            IReferenceDataService referenceDataService, IMessageBus messageBus, ILogService logService)
        {
            _vacancyAboutToExpireThreshold = configurationService.Get<ProcessConfiguration>().VacancyAboutToExpireNotificationHours;
            _vacancyIndexerService = vacancyIndexerService;
            _referenceDataService = referenceDataService;
            _messageBus = messageBus;
            _logService = logService;
        }

        public void Process(ApprenticeshipSummaryUpdate vacancySummaryToIndex)
        {
            PopulateCategoriesCodes(vacancySummaryToIndex);
            _vacancyIndexerService.Index(vacancySummaryToIndex);
            QueueVacancyIfExpiring(vacancySummaryToIndex, _vacancyAboutToExpireThreshold);
        }

        private void PopulateCategoriesCodes(ApprenticeshipSummaryUpdate vacancySummaryToIndex)
        {
            var categories = _referenceDataService.GetCategories();

            if (categories == null)
            {
                _logService.Error("Reference data service return null categories");
                return;
            }

            var category = categories.FirstOrDefault(c => c.FullName == vacancySummaryToIndex.Sector);

            if (category == null)
            {
                vacancySummaryToIndex.SectorCode = "Unknown";
                vacancySummaryToIndex.FrameworkCode = "Unknown";
                _logService.Warn("The vacancy with Id {0} has an unknown Sector and Framework: {1} | {2}. It is likely these were deprecated", vacancySummaryToIndex.Id, vacancySummaryToIndex.Sector, vacancySummaryToIndex.Framework);
            }
            else
            {
                vacancySummaryToIndex.SectorCode = category.CodeName;

                var subCategory = category.SubCategories.FirstOrDefault(sc => sc.FullName == vacancySummaryToIndex.Framework);

                if (subCategory == null)
                {
                    vacancySummaryToIndex.SectorCode = "Unknown";
                    vacancySummaryToIndex.FrameworkCode = "Unknown";
                    _logService.Warn("The vacancy with Id {0} has a mismatched Sector/Framework: {1} | {2}", vacancySummaryToIndex.Id, vacancySummaryToIndex.Sector, vacancySummaryToIndex.Framework);
                }
                else
                {
                    vacancySummaryToIndex.FrameworkCode = subCategory.CodeName;
                }
            }
        }

        public void QueueVacancyIfExpiring(ApprenticeshipSummaryUpdate vacancySummary, int aboutToExpireThreshold)
        {
            try
            {
                if (vacancySummary.ClosingDate < DateTime.Now.AddHours(aboutToExpireThreshold))
                {
                    _logService.Debug("Queueing expiring vacancy");

                    var vacancyAboutToExpireMessage = new VacancyAboutToExpire { Id = vacancySummary.Id };
                    _messageBus.PublishMessage(vacancyAboutToExpireMessage);
                }
            }
            catch (Exception ex)
            {
                _logService.Warn("Failed queueing expiring vacancy {0}", ex, vacancySummary.Id);
            }
        }
    }
}
