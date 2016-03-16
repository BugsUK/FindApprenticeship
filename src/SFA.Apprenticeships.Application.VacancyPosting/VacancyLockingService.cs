namespace SFA.Apprenticeships.Application.VacancyPosting
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Vacancies;
    using Infrastructure.Interfaces;
    using Interfaces.Vacancies;
    using Web.Raa.Common.Configuration;

    // TODO: is this the correct project?
    // TODO: think in a better name
    public class VacancyLockingService : IVacancyLockingService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IConfigurationService _configurationService;

        public VacancyLockingService(IDateTimeService dateTimeService, IConfigurationService configurationService)
        {
            _dateTimeService = dateTimeService;
            _configurationService = configurationService;
        }

        public bool IsVacancyAvailableToQABy(string userName, VacancySummary vacancySummary)
        {
            return NobodyHasTheVacancyLocked(vacancySummary)
                   || VacancyIsLockedByMe(userName, vacancySummary)
                   || AnotherUserHasLeftTheVacanyUnattended(userName, vacancySummary);
        }

        public VacancySummary GetNextAvailableVacancy(string userName, List<VacancySummary> vacancies)
        {
            return vacancies.FirstOrDefault(v => IsVacancyAvailableToQABy(userName, v));
        }

        private static bool VacancyIsLockedByMe(string userName, VacancySummary vacancySummary)
        {
            return vacancySummary.Status == VacancyStatus.ReservedForQA &&
                   vacancySummary.QAUserName == userName;
        }

        private static bool NobodyHasTheVacancyLocked(VacancySummary vacancySummary)
        {
            return vacancySummary.Status == VacancyStatus.Submitted;
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