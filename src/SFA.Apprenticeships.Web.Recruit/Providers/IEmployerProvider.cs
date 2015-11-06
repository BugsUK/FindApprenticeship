namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using ViewModels.VacancyPosting;
    using Raa.Common.ViewModels.Vacancy;

    public interface IEmployerProvider
    {
        EmployerSearchViewModel GetEmployerViewModels(EmployerSearchViewModel searchViewModel);

        EmployerViewModel GetEmployerViewModel(string ern);
    }
}