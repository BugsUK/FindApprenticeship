namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using ViewModels.Employer;

    public interface IEmployerProvider
    {
        EmployerSearchViewModel SearchEmployers(EmployerSearchViewModel searchViewModel);
        EmployerSearchViewModel SearchEdrsEmployers(EmployerSearchViewModel searchViewModel);
        EmployerViewModel GetEmployer(int employerId);
        EmployerViewModel SaveEmployer(EmployerViewModel viewModel);
    }
}