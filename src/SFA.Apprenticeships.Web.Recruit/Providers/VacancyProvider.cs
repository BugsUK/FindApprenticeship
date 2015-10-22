using System.Collections.Generic;
using System.Linq;
using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
using SFA.Apprenticeships.Domain.Interfaces.Repositories;
using SFA.Apprenticeships.Web.Recruit.Converters;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy;
using StructureMap.Building;

namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    public class VacancyProvider : IVacancyProvider
    {
        private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;

        public VacancyProvider(IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository)
        {
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
        }

        public List<VacancyViewModel> GetVacanciesForProvider(string ukprn)
        {
            var vacancies = _apprenticeshipVacancyReadRepository.GetForProvider(ukprn, new List<ProviderVacancyStatuses>() { ProviderVacancyStatuses.Draft });

            return vacancies.Select(v => v.ConvertToVacancyViewModel()).ToList();
        }
    }
}