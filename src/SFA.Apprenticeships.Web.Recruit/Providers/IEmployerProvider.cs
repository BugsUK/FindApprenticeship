
namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;


    public interface IEmployerProvider
    {
        EmployerSearchViewModel GetEmployerViewModels(EmployerSearchViewModel searchViewModel);

        EmployerViewModel GetEmployerViewModel(string ern);
    }
}