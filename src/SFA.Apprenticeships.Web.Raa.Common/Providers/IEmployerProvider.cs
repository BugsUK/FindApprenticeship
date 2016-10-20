
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using ViewModels.Employer;

    public interface IEmployerProvider
    {
        EmployerSearchViewModel GetEmployerViewModels(EmployerSearchViewModel searchViewModel);
    }
}