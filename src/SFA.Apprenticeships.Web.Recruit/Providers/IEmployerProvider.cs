namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public interface IEmployerProvider
    {
        EmployerSearchViewModel GetEmployerViewModels(EmployerSearchViewModel searchViewModel);

        EmployerViewModel GetEmployerViewModel(string ern);
    }
}