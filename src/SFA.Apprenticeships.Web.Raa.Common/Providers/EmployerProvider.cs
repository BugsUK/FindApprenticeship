namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Linq;
    using Application.Interfaces;
    using Application.Interfaces.Employers;
    using Configuration;
    using Converters;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Mappers;
    using ViewModels.Employer;
    using Web.Common.Converters;

    public class EmployerProvider : IEmployerProvider
    {
        private static readonly IMapper Mapper = new RaaCommonWebMappers();

        private readonly IEmployerService _employerService;
        private readonly IConfigurationService _configurationService;

        public EmployerProvider(IEmployerService employerService, IConfigurationService configurationService)
        {
            _employerService = employerService;
            _configurationService = configurationService;
        }

        public EmployerSearchViewModel SearchEmployers(EmployerSearchViewModel searchViewModel)
        {
            var searchParameters = new EmployerSearchParameters
            {
                Id = searchViewModel.Id,
                EdsUrn = searchViewModel.EdsUrn,
                Name = searchViewModel.Name,
                Location = searchViewModel.Location
            };
            var results = _employerService.SearchEmployers(searchParameters);
            var employers = results.Select(e => e.Convert()).ToList();
            searchViewModel.Employers.Page = employers;
            searchViewModel.Employers.ResultsCount = employers.Count;
            return searchViewModel;
        }

        public EmployerSearchViewModel SearchEdrsEmployers(EmployerSearchViewModel searchViewModel)
        {
            var pageSize = _configurationService.Get<RecruitWebConfiguration>().PageSize;
            var resultsPage = _employerService.GetEmployers(searchViewModel.EdsUrn, searchViewModel.Name, searchViewModel.Location, searchViewModel.Employers.CurrentPage, pageSize);
            var resultsViewModelPage = resultsPage.ToViewModel(resultsPage.Page.Select(e => e.Convert()).ToList());
            searchViewModel.Employers = resultsViewModelPage;
            return searchViewModel;
        }

        public EmployerViewModel GetEmployer(int employerId)
        {
            var employer = _employerService.GetEmployer(employerId, false);
            return employer.Convert();
        }

        public EmployerViewModel SaveEmployer(EmployerViewModel viewModel)
        {
            var employer = Mapper.Map<EmployerViewModel, Employer>(viewModel);
            employer = _employerService.SaveEmployer(employer);
            return Mapper.Map<Employer, EmployerViewModel>(employer);
        }
    }
}