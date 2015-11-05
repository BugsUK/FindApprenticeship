namespace SFA.Apprenticeships.Web.Manage.Providers
{
    namespace SFA.Apprenticeships.Web.Recruit.Providers
    {
        using System.Collections.Generic;
        using System.Linq;
        using Application.Interfaces.Providers;
        using Domain.Entities.Vacancies.ProviderVacancies;
        using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
        using Domain.Interfaces.Repositories;
        using ViewModels;

        public class VacancyProvider : IVacancyProvider
        {
            private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;
            private readonly IProviderService _providerService;

            public VacancyProvider(IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository, IProviderService providerService)
            {
                _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
                _providerService = providerService;
            }

            public List<VacancySummaryViewModel> GetPendingQAVacancies()
            {
                var vacancies =
                    _apprenticeshipVacancyReadRepository.GetWithStatus(new List<ProviderVacancyStatuses>
                    {
                        ProviderVacancyStatuses.PendingQA
                    });

                return vacancies.Select(ConvertToVacancySummaryViewModel).ToList();
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
                    VacancyReferenceNumber = apprenticeshipVacancy.VacancyReferenceNumber
                };
            }
        }
    }
}