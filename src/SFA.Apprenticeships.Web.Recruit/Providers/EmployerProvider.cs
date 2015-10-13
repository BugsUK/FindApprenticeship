namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Collections.Generic;
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

        public EmployerFilterViewModel GetEmployerViewModels(EmployerFilterViewModel filterViewModel)
        {
            //TODO: Wire through

            return filterViewModel;
        }

        public EmployerSearchViewModel GetEmployerViewModels(EmployerSearchViewModel searchViewModel)
        {
            return new EmployerSearchViewModel { EmployerResults = new List<EmployerResultViewModel>() };
        }

        public EmployerViewModel GetEmployerViewModel(string ern)
        {
            var employer = _employerService.GetEmployer(ern);

            return employer.Convert();
        }
    }
}