namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public interface IEmployerProvider
    {
        EmployerFilterViewModel GetEmployerViewModels(EmployerFilterViewModel filterViewModel);

        EmployerSearchViewModel GetEmployerViewModels(EmployerSearchViewModel searchViewModel);

        EmployerViewModel GetEmployerViewModel(string ern);
    }
}