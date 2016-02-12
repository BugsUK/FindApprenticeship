namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Queries;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Infrastructure.Interfaces;

    public class VacancyStatusProcessor : IVacancyStatusProcessor
    {
        private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;
        private readonly IApprenticeshipVacancyWriteRepository _apprenticeshipVacancyWriteRepository;
        private readonly IServiceBus _serviceBus;
        private readonly ILogService _logService;

        public VacancyStatusProcessor(
            IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository,
            IApprenticeshipVacancyWriteRepository apprenticeshipVacancyWriteRepository,
            IServiceBus serviceBus,
            ILogService logService)
        {
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
            _apprenticeshipVacancyWriteRepository = apprenticeshipVacancyWriteRepository;
            _serviceBus = serviceBus;
            _logService = logService;
        }

        /// <summary>
        /// This queues up to 1000 vacancies for 
        /// </summary>
        /// <param name="deadline"></param>
        public void QueueVacanciesForClosure(DateTime deadline)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var query = new ApprenticeshipVacancyQuery() {
                CurrentPage = 1,
                DesiredStatuses = new List<ProviderVacancyStatuses>() { ProviderVacancyStatuses.Live },
                LatestClosingDate = deadline,
                PageSize = 1000 };

            int resultCount;

            var eligibleVacancies = _apprenticeshipVacancyReadRepository.Find(query, out resultCount);

            var message = string.Format("Querying vacancies about to close took {0}", stopwatch.Elapsed);

            var counter = 0;

            foreach (var vacancy in eligibleVacancies)
            {
                var eligibleForClosure = new VacancyEligibleForClosure(vacancy.VacancyId);
                _serviceBus.PublishMessage(eligibleForClosure);
                counter ++;
            }

            stopwatch.Stop();
            message += string.Format(". Queuing {0} vacancies for closure took {1}", counter, stopwatch.Elapsed);
            if (stopwatch.ElapsedMilliseconds > 60000)
            {
                _logService.Warn(message);
            }
            else
            {
                _logService.Info(message);
            }
        }

        public void ProcessVacancyClosure(VacancyEligibleForClosure vacancyEligibleForClosure)
        {
            var vacancy = _apprenticeshipVacancyReadRepository.Get(vacancyEligibleForClosure.EntityId);
            
            switch (vacancy.Status)
            {
                case ProviderVacancyStatuses.Live:
                    vacancy.Status = ProviderVacancyStatuses.Closed;
                    _apprenticeshipVacancyWriteRepository.DeepSave(vacancy);
                    break;
                default:
                    return;
            }
        }
    }
}
