namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using System.Collections.Generic;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public interface IEmployerProvider
    {
        IEnumerable<EmployerViewModel> GetEmployers(string providerSiteErn);

        EmployerFilterViewModel GetEmployers(EmployerFilterViewModel filterViewModel);

        EmployerSearchViewModel GetEmployers(EmployerSearchViewModel searchViewModel);

        EmployerViewModel GetEmployer(string providerSiteErn, string ern);
    }
}