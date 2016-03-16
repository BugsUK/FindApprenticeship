namespace SFA.Apprenticeships.Application.VacancyPosting
{
    using Domain.Entities.Raa.Vacancies;
    using Infrastructure.Interfaces;
    using Interfaces.Vacancies;
    using Web.Raa.Common.Configuration;

    // TODO: is this the correct project?
    public class VacancyLockingService : IVacancyLockingService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IConfigurationService _configurationService;

        public VacancyLockingService(IDateTimeService dateTimeService, IConfigurationService configurationService)
        {
            _dateTimeService = dateTimeService;
            _configurationService = configurationService;
        }

        /*
        *   TODO: In the original code we check that the vacany status is correct.
        *   I think we can assume that if the QAUserName property is filled (or not)
        *   The vacancy is in the correct status    
        */
        public bool CanBeReservedForQABy(string userName, VacancySummary vacancySummary)
        {
            return string.IsNullOrWhiteSpace(vacancySummary.QAUserName)
                   || vacancySummary.QAUserName == userName
                   || AnotherUserHasLeftTheVacanyUnattended(userName, vacancySummary);
        }

        private bool AnotherUserHasLeftTheVacanyUnattended(string userName, VacancySummary vacancySummary)
        {
            var timeout = _configurationService.Get<ManageWebConfiguration>().QAVacancyTimeout; // In minutes
            return vacancySummary.QAUserName != userName &&
                vacancySummary.DateStartedToQA.HasValue &&
                (_dateTimeService.UtcNow - vacancySummary.DateStartedToQA.Value).TotalMinutes > timeout;
        }
    }
}