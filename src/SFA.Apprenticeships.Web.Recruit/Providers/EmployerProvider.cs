namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Linq;
    using Application.Interfaces.Employers;
    using Common.Converters;
    using Configuration;
    using Converters;
    using Domain.Interfaces.Configuration;
    using ViewModels.VacancyPosting;
    using Raa.Common.ViewModels.Vacancy;

    public class EmployerProvider : IEmployerProvider
    {
        private readonly IEmployerService _employerService;
        private readonly IConfigurationService _configurationService;

        public EmployerProvider(IEmployerService employerService, IConfigurationService configurationService)
        {
            _employerService = employerService;
            _configurationService = configurationService;
        }

        public EmployerSearchViewModel GetEmployerViewModels(EmployerSearchViewModel searchViewModel)
        {
            var pageSize = _configurationService.Get<RecruitWebConfiguration>().PageSize;
            var resultsPage = _employerService.GetEmployers(searchViewModel.Ern, searchViewModel.Name, searchViewModel.Location, searchViewModel.EmployerResultsPage.CurrentPage, pageSize);
            var resultsViewModelPage = resultsPage.ToViewModel(resultsPage.Page.Select(e => e.ConvertToResult()).ToList());
            searchViewModel.EmployerResultsPage = resultsViewModelPage;
            return searchViewModel;
        }

        public EmployerViewModel GetEmployerViewModel(string ern)
        {
            var employer = _employerService.GetEmployer(ern);

            return employer.Convert();
        }
    }
}