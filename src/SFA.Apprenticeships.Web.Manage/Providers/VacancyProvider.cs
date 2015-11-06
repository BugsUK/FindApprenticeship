namespace SFA.Apprenticeships.Web.Manage.Providers
{
    namespace SFA.Apprenticeships.Web.Recruit.Providers
    {
        using System.Collections.Generic;
        using System.Linq;
        using Application.Interfaces.DateTime;
        using Application.Interfaces.Providers;
        using Configuration;
        using Domain.Entities.Vacancies.ProviderVacancies;
        using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
        using Domain.Interfaces.Configuration;
        using Domain.Interfaces.Repositories;
        using ViewModels;

        public class VacancyProvider : IVacancyProvider
        {
            private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;
            private readonly IApprenticeshipVacancyWriteRepository _apprenticeshipVacancyWriteRepository;
            private readonly IProviderService _providerService;
            private readonly IDateTimeService _dateTimeService;
            private readonly IConfigurationService _configurationService;

            public VacancyProvider(IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository,
                IApprenticeshipVacancyWriteRepository apprenticeshipVacancyWriteRepository,
                IProviderService providerService, IDateTimeService dateTimeService,
                IConfigurationService configurationService)
            {
                _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
                _apprenticeshipVacancyWriteRepository = apprenticeshipVacancyWriteRepository;
                _providerService = providerService;
                _dateTimeService = dateTimeService;
                _configurationService = configurationService;
            }

            public List<VacancySummaryViewModel> GetPendingQAVacancies()
            {
                var vacancies =
                    _apprenticeshipVacancyReadRepository.GetWithStatus(new List<ProviderVacancyStatuses>
                    {
                        ProviderVacancyStatuses.PendingQA
                    });

                return vacancies.Where(VacancyReadyToQA).Select(ConvertToVacancySummaryViewModel).ToList();
            }

            private bool VacancyReadyToQA(ApprenticeshipVacancy apprenticeshipVacancy)
            {
                var timeout = _configurationService.Get<ManageWebConfiguration>().QAVacancyTimeout; //In minutes
                if (NoUserHasStartedToQATheVacancy(apprenticeshipVacancy))
                {
                    return true;
                }

                if (AUserHasLeftTheVacancyUnattended(apprenticeshipVacancy, timeout))
                {
                    return true;
                }

                return false;
            }

            private bool AUserHasLeftTheVacancyUnattended(ApprenticeshipVacancy apprenticeshipVacancy, int timeout)
            {
                return (_dateTimeService.UtcNow() - apprenticeshipVacancy.DateStartedToQA).Value.TotalMinutes > timeout;
            }

            private static bool NoUserHasStartedToQATheVacancy(ApprenticeshipVacancy apprenticeshipVacancy)
            {
                return string.IsNullOrWhiteSpace(apprenticeshipVacancy.QAUserName) || !apprenticeshipVacancy.DateStartedToQA.HasValue;
            }

            public void ApproveVacancy(long vacancyReferenceNumber)
            {
                var vacancy = _apprenticeshipVacancyReadRepository.Get(vacancyReferenceNumber);
                vacancy.Status = ProviderVacancyStatuses.Live;

                _apprenticeshipVacancyWriteRepository.Save(vacancy);
            }

            private VacancySummaryViewModel ConvertToVacancySummaryViewModel(ApprenticeshipVacancy apprenticeshipVacancy)
            {
                var provider = _providerService.GetProvider(apprenticeshipVacancy.Ukprn);

                return new VacancySummaryViewModel
                {
                    ClosingDate = apprenticeshipVacancy.ClosingDate.Value,
                    DateSubmitted = apprenticeshipVacancy.DateSubmitted.Value,
                    ProviderName = provider.Name,
                    Status = apprenticeshipVacancy.Status,
                    Title = apprenticeshipVacancy.Title,
                    VacancyReferenceNumber = apprenticeshipVacancy.VacancyReferenceNumber,
                    DateStartedToQA = apprenticeshipVacancy.DateStartedToQA,
                    QAUserName = apprenticeshipVacancy.QAUserName
                };
            }
        }
    }
}