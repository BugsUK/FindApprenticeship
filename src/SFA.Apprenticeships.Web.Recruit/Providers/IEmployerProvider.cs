namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Collections.Generic;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public interface IEmployerProvider
    {
        IEnumerable<EmployerViewModel> GetEmployerViewModels(string providerSiteErn);

        EmployerFilterViewModel GetEmployerViewModels(EmployerFilterViewModel filterViewModel);

        EmployerSearchViewModel GetEmployerViewModels(EmployerSearchViewModel searchViewModel);

        EmployerViewModel GetEmployerViewModel(string providerSiteErn, string ern);

        EmployerViewModel ConfirmEmployer(string providerSiteErn, string ern, string description);
    }
}