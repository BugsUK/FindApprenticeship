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
    using Extensions;

    public class ApprenticeshipSummaryUpdateProcessor : IApprenticeshipSummaryUpdateProcessor
    {
        private readonly IReferenceDataService _referenceDataService;
        private readonly IMessageBus _messageBus;
        private readonly ILogService _logService;
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, Elastic.ApprenticeshipSummary> _vacancyIndexerService;
        private readonly int _vacancyAboutToExpireThreshold;
        private readonly bool _strictEtlValidation;

        public ApprenticeshipSummaryUpdateProcessor(
            IConfigurationService configurationService,
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, Elastic.ApprenticeshipSummary> vacancyIndexerService,
            IReferenceDataService referenceDataService, IMessageBus messageBus, ILogService logService)
        {
            _vacancyIndexerService = vacancyIndexerService;
            _referenceDataService = referenceDataService;
            _messageBus = messageBus;
            _logService = logService;
            _vacancyAboutToExpireThreshold = configurationService.Get<ProcessConfiguration>().VacancyAboutToExpireNotificationHours;
            _strictEtlValidation = configurationService.Get<ProcessConfiguration>().StrictEtlValidation;
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

            var category = categories.FirstOrDefault(c => c.FullName == vacancySummaryToIndex.Category);

            if (category == null)
            {
                vacancySummaryToIndex.CategoryCode = "Unknown";
                vacancySummaryToIndex.SubCategoryCode = "Unknown";
                _logService.Warn(_strictEtlValidation, "The vacancy with Id {0} has an unknown Category and SubCategory: {1} | {2}. It is likely these were deprecated", vacancySummaryToIndex.Id, vacancySummaryToIndex.Category, vacancySummaryToIndex.SubCategory);
            }
            else
            {
                vacancySummaryToIndex.CategoryCode = category.CodeName;

                var subCategory = category.SubCategories.FirstOrDefault(sc => sc.FullName == vacancySummaryToIndex.SubCategory);

                if (subCategory == null)
                {
                    vacancySummaryToIndex.CategoryCode = "Unknown";
                    vacancySummaryToIndex.SubCategoryCode = "Unknown";
                    _logService.Warn(_strictEtlValidation, "The vacancy with Id {0} has a mismatched Category/SubCategory: {1} | {2}", vacancySummaryToIndex.Id, vacancySummaryToIndex.Category, vacancySummaryToIndex.SubCategory);
                }
                else
                {
                    vacancySummaryToIndex.SubCategoryCode = subCategory.CodeName;
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
