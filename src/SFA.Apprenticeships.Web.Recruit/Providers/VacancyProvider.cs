using SFA.Apprenticeships.Web.Raa.Common.Converters;

namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Converters;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Interfaces.Repositories;
    using Raa.Common.ViewModels.Vacancy;

    public class VacancyProvider : IVacancyProvider
    {
        private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;

        public VacancyProvider(IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository)
        {
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
        }

        public List<VacancyViewModel> GetVacanciesForProvider(string ukprn)
        {
            var vacancies = _apprenticeshipVacancyReadRepository.GetForProvider(ukprn,
                new List<ProviderVacancyStatuses> {ProviderVacancyStatuses.Draft});

            return vacancies.Select(v => v.ConvertToVacancyViewModel()).ToList();
        }
    }
}