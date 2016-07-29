namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using SFA.Infrastructure.Interfaces;
    using Vacancy;
    using ErrorCodes = Interfaces.Vacancies.ErrorCodes;

    public class GetCandidateVacancyDetailStrategy<TVacancyDetail> : ILegacyGetCandidateVacancyDetailStrategy<TVacancyDetail>
        where TVacancyDetail : VacancyDetail
    {
        private readonly ILogService _logger;

        private readonly IVacancyDataProvider<TVacancyDetail> _vacancyDataProvider;

        public GetCandidateVacancyDetailStrategy(
            ILogService logger,
            IVacancyDataProvider<TVacancyDetail> vacancyDataProvider)
        {
            _logger = logger;
            _vacancyDataProvider = vacancyDataProvider;
        }

        public TVacancyDetail GetVacancyDetails(Guid candidateId, int vacancyId)
        {
            _logger.Debug("Calling LegacyGetCandidateVacancyDetailStrategy to get vacancy details for vacancy ID {0} and candidate ID {1}.", vacancyId, candidateId);

            try
            {
                var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

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
