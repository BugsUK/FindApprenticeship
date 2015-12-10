namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using Application.Interfaces.Logging;
    using Application.Interfaces.VacancyPosting;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using ViewModels.Application;

    public class ApplicationProvider : IApplicationProvider
    {
        private readonly IConfigurationService _configurationService;
        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public ApplicationProvider(IConfigurationService configurationService, IVacancyPostingService vacancyPostingService, IMapper mapper, ILogService logService)
        {
            _configurationService = configurationService;
            _vacancyPostingService = vacancyPostingService;
            _mapper = mapper;
            _logService = logService;
        }

        public VacancyApplicationsViewModel GetVacancyApplicationsViewModel(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);
            var viewModel = _mapper.Map<ApprenticeshipVacancy, VacancyApplicationsViewModel>(vacancy);
            return viewModel;
        }
    }
}