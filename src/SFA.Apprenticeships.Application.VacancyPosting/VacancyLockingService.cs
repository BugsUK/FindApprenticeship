namespace SFA.Apprenticeships.Application.VacancyPosting
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Vacancies;
    using SFA.Infrastructure.Interfaces;
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
            return
                vacancies.OrderBy(v => v.DateSubmitted)
                    .FirstOrDefault(IsVacancySuitableToBeTheNextVacancyToChoose);
        }

        private bool IsVacancySuitableToBeTheNextVacancyToChoose(VacancySummary vacancySummary)
        {
            return NobodyHasTheVacancyLocked(vacancySummary)
                   || VacancyIsUnattended(vacancySummary);
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
            return vacancySummary.QAUserName != userName && VacancyIsUnattended(vacancySummary);
        }

        private bool VacancyIsUnattended(VacancySummary vacancySummary)
        {
            var timeout = _configurationService.Get<ManageWebConfiguration>().QAVacancyTimeout; // In minutes
            return vacancySummary.DateStartedToQA.HasValue &&
                   (_dateTimeService.UtcNow - vacancySummary.DateStartedToQA.Value).TotalMinutes > timeout;
        }
    }
}