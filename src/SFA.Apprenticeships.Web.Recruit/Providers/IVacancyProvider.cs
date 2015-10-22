using System;
using System.Collections.Generic;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy;

namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    public interface IVacancyProvider
    {
        List<VacancyViewModel> GetVacanciesForProvider(string ukprn);
    }
}
