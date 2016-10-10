namespace SFA.Apprenticeships.Infrastructure.Raa.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.ReferenceData;
    using Application.Vacancies.Entities;
    using Application.VacancyPosting.Strategies;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Messaging;
    using Domain.Raa.Interfaces.Repositories;
    using Mappers;

    public class PublishVacancySummaryUpdateStrategy : IPublishVacancySummaryUpdateStrategy
    {
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IProviderService _providerService;
        private readonly IEmployerService _employerService;
        private readonly IReferenceDataProvider _referenceDataProvider;
        private readonly IMapper _mapper;
        private readonly IServiceBus _serviceBus;
        private readonly ILogService _logService;

        public PublishVacancySummaryUpdateStrategy(
            IVacancyReadRepository vacancyReadRepository, 
            IProviderService providerService, 
            IEmployerService employerService, 
            IReferenceDataProvider referenceDataProvider, 
            IMapper mapper, 
            IServiceBus serviceBus, 
            ILogService logService)
        {
            _vacancyReadRepository = vacancyReadRepository;
            _providerService = providerService;
            _employerService = employerService;
            _referenceDataProvider = referenceDataProvider;
            _mapper = mapper;
            _serviceBus = serviceBus;
            _logService = logService;
        }

        public void PublishVacancySummaryUpdate(Vacancy vacancy)
        {
            if (vacancy == null) return;

            try
            {
                if (vacancy.Status == VacancyStatus.Live)
                {
                    var vacancySummary = _vacancyReadRepository.GetByIds(new List<int> { vacancy.VacancyId }).Single();
                    var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationships(new List<int> { vacancySummary.VacancyOwnerRelationshipId }, false).Single().Value;
                    var employer = _employerService.GetEmployers(new List<int> { vacancyOwnerRelationship.EmployerId }).Single();
                    var providers = _providerService.GetProviders(new List<int> { vacancy.ProviderId }).Single();
                    var categories = _referenceDataProvider.GetCategories().ToList();

                    if (vacancy.VacancyType == VacancyType.Apprenticeship)
                    {
                        _logService.Info($"Publishing apprenticeship summary message for vacancy id {vacancy.VacancyId}");
                        var apprenticeshipSummary = ApprenticeshipSummaryMapper.GetApprenticeshipSummary(vacancySummary, employer, providers, categories, _logService);
                        var apprenticeshipSummaryUpdate = _mapper.Map<ApprenticeshipSummary, ApprenticeshipSummaryUpdate>(apprenticeshipSummary);
                        apprenticeshipSummaryUpdate.UseAlias = true;
                        _serviceBus.PublishMessage(apprenticeshipSummaryUpdate);
                        _logService.Info($"Published apprenticeship summary message for vacancy id {vacancy.VacancyId}");
                    }

                    if (vacancy.VacancyType == VacancyType.Traineeship)
                    {
                        _logService.Info($"Publishing traineeship summary message for vacancy id {vacancy.VacancyId}");
                        var traineeshipSummary = TraineeshipSummaryMapper.GetTraineeshipSummary(vacancySummary, employer, providers, categories, _logService);
                        var traineeshipSummaryUpdate = _mapper.Map<TraineeshipSummary, TraineeshipSummaryUpdate>(traineeshipSummary);
                        traineeshipSummaryUpdate.UseAlias = true;
                        _serviceBus.PublishMessage(traineeshipSummaryUpdate);
                        _logService.Info($"Published traineeship summary message for vacancy id {vacancy.VacancyId}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.Warn($"Failed to publish vacancy summary message for vacancy id {vacancy.VacancyId}", ex);
            }
        }

    }
}