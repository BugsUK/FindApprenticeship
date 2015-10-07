namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Employers;
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

        public IEnumerable<EmployerViewModel> GetEmployers(string ern)
        {
            var employers = _employerService.GetEmployers(ern);

            return employers.Select(Convert);
        }

        public EmployerResultsViewModel GetEmployers(string ern, EmployerFilterViewModel filterViewModel)
        {
            var employers = _employerService.GetEmployers(ern);

            return new EmployerResultsViewModel { EmployerResults = employers.Select(ConvertToResult).ToList() };
        }

        public EmployerResultsViewModel GetEmployers(EmployerSearchViewModel searchViewModel)
        {
            return new EmployerResultsViewModel { EmployerResults = new List<EmployerResultViewModel>() };
        }

        private static EmployerResultViewModel ConvertToResult(Employer employer)
        {
            var viewModel = new EmployerResultViewModel
            {
                Ern = employer.Ern,
                //EmployerId = 
                EmployerName = employer.Name,
                //Address = 
            };

            return viewModel;
        }

        private static EmployerViewModel Convert(Employer employer)
        {
            var viewModel = new EmployerViewModel
            {
                Ern = employer.Ern,
                Name = employer.Name,
                Description = employer.Description,
                Website = employer.Website,
                //Address = 
            };

            return viewModel;
        }
    }
}