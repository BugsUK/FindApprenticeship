
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    public interface IEmployerProvider
    {
        EmployerSearchViewModel GetEmployerViewModels(EmployerSearchViewModel searchViewModel);

        EmployerViewModel GetEmployerViewModel(string edsUrn);
    }
}