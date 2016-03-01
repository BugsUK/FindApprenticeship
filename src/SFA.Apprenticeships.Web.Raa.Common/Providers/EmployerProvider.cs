using System.Linq;
using SFA.Apprenticeships.Application.Interfaces.Employers;
using SFA.Infrastructure.Interfaces;
using SFA.Apprenticeships.Web.Common.Converters;
using SFA.Apprenticeships.Web.Raa.Common.Configuration;
using SFA.Apprenticeships.Web.Raa.Common.Converters;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
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
            var resultsPage = _employerService.GetEmployers(searchViewModel.EdsUrn, searchViewModel.Name, searchViewModel.Location, searchViewModel.EmployerResultsPage.CurrentPage, pageSize);
            var resultsViewModelPage = resultsPage.ToViewModel(resultsPage.Page.Select(e => e.ConvertToResult()).ToList());
            searchViewModel.EmployerResultsPage = resultsViewModelPage;
            return searchViewModel;
        }
    }
}