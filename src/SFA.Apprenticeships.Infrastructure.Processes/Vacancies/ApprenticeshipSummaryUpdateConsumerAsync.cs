namespace SFA.Apprenticeships.Infrastructure.Processes.Vacancies
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Application.Interfaces.ReferenceData;
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using EasyNetQ.AutoSubscribe;
    using VacancyIndexer;
    using Elastic = Elastic.Common.Entities;

    public class ApprenticeshipSummaryUpdateConsumerAsync : IConsumeAsync<ApprenticeshipSummaryUpdate>
    {
        private readonly IReferenceDataService _referenceDataService;
        private readonly ILogService _logService;
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, Elastic.ApprenticeshipSummary> _vacancyIndexerService;
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;
        private readonly int _vacancyAboutToExpireThreshold;

        public ApprenticeshipSummaryUpdateConsumerAsync(
            IConfigurationService configurationService,
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, Elastic.ApprenticeshipSummary> vacancyIndexerService,
            IVacancySummaryProcessor vacancySummaryProcessor, IReferenceDataService referenceDataService, ILogService logService)
        {
            _vacancyAboutToExpireThreshold = configurationService.Get<ProcessConfiguration>(ProcessConfiguration.ConfigurationName).VacancyAboutToExpireNotificationHours;
            _vacancyIndexerService = vacancyIndexerService;
            _vacancySummaryProcessor = vacancySummaryProcessor;
            _referenceDataService = referenceDataService;
            _logService = logService;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "ApprenticeshipSummaryUpdateConsumerAsync")]
        public Task Consume(ApprenticeshipSummaryUpdate vacancySummaryToIndex)
        {
            return Task.Run(() =>
            {
                PopulateCategoriesCodes(vacancySummaryToIndex);
                _vacancyIndexerService.Index(vacancySummaryToIndex);
                _vacancySummaryProcessor.QueueVacancyIfExpiring(vacancySummaryToIndex, _vacancyAboutToExpireThreshold);
            });
        }

        private void PopulateCategoriesCodes(ApprenticeshipSummaryUpdate vacancySummaryToIndex)
        {
            var categories = _referenceDataService.GetCategories().ToArray();

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
    }
}
