namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Interfaces.Messaging;
    using Domain.Raa.Interfaces.Queries;
    using Domain.Raa.Interfaces.Repositories;
    using Entities;
    using SFA.Infrastructure.Interfaces;

    public class VacancyStatusProcessor : IVacancyStatusProcessor
    {
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IServiceBus _serviceBus;
        private readonly ILogService _logService;

        public VacancyStatusProcessor(
            IVacancyReadRepository vacancyReadRepository,
            IVacancyWriteRepository vacancyWriteRepository,
            IServiceBus serviceBus,
            ILogService logService)
        {
            _vacancyReadRepository = vacancyReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
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
                DesiredStatuses = new List<VacancyStatus>() { VacancyStatus.Live },
                LatestClosingDate = deadline,
                EditedInRaa = true,
                PageSize = 1000 };

            int resultCount;

            var eligibleVacancies = _vacancyReadRepository.Find(query, out resultCount);

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
            var vacancy = _vacancyReadRepository.Get(vacancyEligibleForClosure.VacancyId);
            
            switch (vacancy.Status)
            {
                case VacancyStatus.Live:
                    vacancy.Status = VacancyStatus.Closed;
                    _vacancyWriteRepository.Update(vacancy);
                    break;
                default:
                    return;
            }
        }
    }
}
