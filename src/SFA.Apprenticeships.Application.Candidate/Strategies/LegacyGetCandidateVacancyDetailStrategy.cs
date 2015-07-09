﻿namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Applications;
    using Applications.Entities;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Messaging;
    using Interfaces.Logging;
    using Vacancy;
    using ErrorCodes = Interfaces.Vacancies.ErrorCodes;

    public class LegacyGetCandidateVacancyDetailStrategy<TVacancyDetail> : ILegacyGetCandidateVacancyDetailStrategy<TVacancyDetail>
        where TVacancyDetail : VacancyDetail
    {
        private readonly ILogService _logger;

        private readonly IVacancyDataProvider<TVacancyDetail> _vacancyDataProvider;
        private readonly IApplicationVacancyUpdater _applicationVacancyUpdater;
        private readonly IServiceBus _serviceBus;

        public LegacyGetCandidateVacancyDetailStrategy(
            ILogService logger,
            IServiceBus serviceBus,
            IVacancyDataProvider<TVacancyDetail> vacancyDataProvider,
            IApplicationVacancyUpdater applicationVacancyUpdater)
        {
            _logger = logger;
            _serviceBus = serviceBus;
            _vacancyDataProvider = vacancyDataProvider;
            _applicationVacancyUpdater = applicationVacancyUpdater;
        }

        public TVacancyDetail GetVacancyDetails(Guid candidateId, int vacancyId)
        {
            _logger.Debug("Calling LegacyGetCandidateVacancyDetailStrategy to get vacancy details for vacancy ID {0} and candidate ID {1}.", vacancyId, candidateId);

            try
            {
                var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

                if (vacancyDetails != null)
                {
                    // update the application for this candidate (if they have one) with latest info from legacy
                    _applicationVacancyUpdater.Update(candidateId, vacancyId, vacancyDetails);

                    // propagate the current vacancy status for other consumers
                    var vacancyStatusSummary = new VacancyStatusSummary
                    {
                        LegacyVacancyId = vacancyId,
                        VacancyStatus = vacancyDetails.VacancyStatus,
                        ClosingDate = vacancyDetails.ClosingDate
                    };

                    _serviceBus.PublishMessage(vacancyStatusSummary);
                }

                return vacancyDetails;
            }
            catch (Exception e)
            {
                var message = string.Format("Get vacancy failed for vacancy ID {0} and candidate ID {1}.", vacancyId, candidateId);

                _logger.Debug(message, e);

                throw new CustomException(message, e, ErrorCodes.GetVacancyDetailsFailed);
            }
        }
    }
}
