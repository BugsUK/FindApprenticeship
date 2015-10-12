namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Employers;
    using Converters;
    using Domain.Entities.Organisations;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public class EmployerProvider : IEmployerProvider
    {
        private readonly IEmployerService _employerService;

        public EmployerProvider(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        public IEnumerable<EmployerViewModel> GetEmployerViewModels(string providerSiteErn)
        {
            var employers = _employerService.GetEmployers(providerSiteErn);

            return employers.Select(e => e.Convert());
        }

        public EmployerFilterViewModel GetEmployerViewModels(EmployerFilterViewModel filterViewModel)
        {
            var employers = _employerService.GetEmployers(filterViewModel.ProviderSiteErn);
            filterViewModel.EmployerResults = employers.Select(e => e.ConvertToResult()).ToList();

            return filterViewModel;
        }

        public EmployerSearchViewModel GetEmployerViewModels(EmployerSearchViewModel searchViewModel)
        {
            return new EmployerSearchViewModel { EmployerResults = new List<EmployerResultViewModel>() };
        }

        public EmployerViewModel GetEmployerViewModel(string providerSiteErn, string ern)
        {
            var employer = _employerService.GetEmployer(providerSiteErn, ern);

            return employer.Convert();
        }

        public EmployerViewModel ConfirmEmployer(EmployerViewModel viewModel)
        {
            var employer = _employerService.GetEmployer(viewModel.ProviderSiteErn, viewModel.Ern);
            employer.Description = viewModel.Description;
            employer = _employerService.SaveEmployer(employer);
            return employer.Convert();
        }
    }
}