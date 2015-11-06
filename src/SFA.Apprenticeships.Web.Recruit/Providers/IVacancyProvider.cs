namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Collections.Generic;
    using Raa.Common.ViewModels.Vacancy;

    public interface IVacancyProvider
    {
        List<VacancyViewModel> GetVacanciesForProvider(string ukprn);
    }
}
