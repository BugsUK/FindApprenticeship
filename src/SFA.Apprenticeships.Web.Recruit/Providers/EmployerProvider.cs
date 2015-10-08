namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
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

        public IEnumerable<EmployerViewModel> GetEmployers(string providerSiteErn)
        {
            var employers = _employerService.GetEmployers(providerSiteErn);

            return employers.Select(Convert);
        }

        public EmployerFilterViewModel GetEmployers(EmployerFilterViewModel filterViewModel)
        {
            var employers = _employerService.GetEmployers(filterViewModel.ProviderSiteErn);
            filterViewModel.EmployerResults = employers.Select(ConvertToResult).ToList();

            return filterViewModel;
        }

        public EmployerSearchViewModel GetEmployers(EmployerSearchViewModel searchViewModel)
        {
            return new EmployerSearchViewModel { EmployerResults = new List<EmployerResultViewModel>() };
        }

        public EmployerViewModel GetEmployer(string providerSiteErn, string ern)
        {
            var employer = _employerService.GetEmployer(providerSiteErn, ern);

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