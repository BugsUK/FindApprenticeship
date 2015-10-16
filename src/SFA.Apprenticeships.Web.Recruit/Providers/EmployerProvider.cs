namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Linq;
    using Application.Interfaces.Employers;
    using Converters;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public class EmployerProvider : IEmployerProvider
    {
        private readonly IEmployerService _employerService;

        public EmployerProvider(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        public EmployerSearchViewModel GetEmployerViewModels(EmployerSearchViewModel searchViewModel)
        {
            var results = _employerService.GetEmployers(searchViewModel.Ern, searchViewModel.Name, searchViewModel.Location);
            var employerResultViewModels = results.Select(e => e.ConvertToResult()).ToList();
            searchViewModel.EmployerResults = employerResultViewModels;
            searchViewModel.ResultsCount = employerResultViewModels.Count;
            return searchViewModel;
        }

        public EmployerViewModel GetEmployerViewModel(string ern)
        {
            var employer = _employerService.GetEmployer(ern);

            return employer.Convert();
        }
    }
}