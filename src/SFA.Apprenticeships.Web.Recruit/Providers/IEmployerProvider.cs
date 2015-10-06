namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Collections.Generic;
    using ViewModels.Vacancy;

    public interface IEmployerProvider
    {
        IEnumerable<EmployerViewModel> GetEmployers(string ern);
    }
}