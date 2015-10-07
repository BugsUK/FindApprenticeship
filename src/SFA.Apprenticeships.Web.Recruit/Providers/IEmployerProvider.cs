namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using System.Collections.Generic;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public interface IEmployerProvider
    {
        IEnumerable<EmployerViewModel> GetEmployers(string ern);

        EmployerResultsViewModel GetEmployers(Guid providerId, EmployerFilterViewModel filterViewModel);

        EmployerResultsViewModel GetEmployers(EmployerSearchViewModel searchViewModel);
    }
}