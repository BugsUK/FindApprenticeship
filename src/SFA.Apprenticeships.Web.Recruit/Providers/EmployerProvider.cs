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

            return employers.Select(Convert);
        }

        public EmployerFilterViewModel GetEmployerViewModels(EmployerFilterViewModel filterViewModel)
        {
            var employers = _employerService.GetEmployers(filterViewModel.ProviderSiteErn);
            filterViewModel.EmployerResults = employers.Select(ConvertToResult).ToList();

            return filterViewModel;
        }

        public EmployerSearchViewModel GetEmployerViewModels(EmployerSearchViewModel searchViewModel)
        {
            return new EmployerSearchViewModel { EmployerResults = new List<EmployerResultViewModel>() };
        }

        public EmployerViewModel GetEmployerViewModel(string providerSiteErn, string ern)
        {
            var employer = _employerService.GetEmployer(providerSiteErn, ern);

            return Convert(employer);
        }

        public EmployerViewModel ConfirmEmployer(string providerSiteErn, string ern, string description)
        {
            var employer = _employerService.GetEmployer(providerSiteErn, ern);
            employer = _employerService.SaveEmployer(employer);
            return Convert(employer);
        }

        private static EmployerResultViewModel ConvertToResult(Employer employer)
        {
            var viewModel = new EmployerResultViewModel
            {
                Ern = employer.Ern,
                //EmployerId = 
                EmployerName = employer.Name,
                Address = employer.Address.Convert()
            };

            return viewModel;
        }

        private static EmployerViewModel Convert(Employer employer)
        {
            var viewModel = new EmployerViewModel
            {
                ProviderSiteErn = employer.ProviderSiteErn,
                Ern = employer.Ern,
                Name = employer.Name,
                Description = employer.Description,
                Website = employer.Website,
                Address = employer.Address.Convert()
            };

            return viewModel;
        }
    }
}