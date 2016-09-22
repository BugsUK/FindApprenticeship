namespace SFA.Apprenticeships.Web.Raa.Common.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;
    using ViewModels;
    using ViewModels.ProviderUser;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels;

    //[Obsolete("This is a refactoring of the VacancyProvider and has not been fully implemented. It's existance is purely for the POC Web Api project (do not delete!)")]
    public interface IVacancySummaryStrategy
    {
        PageableViewModel<VacancySummaryViewModel> GetVacancySummaries(string query, int providerSiteId, int providerId, VacancyType vacancyType, VacanciesSummaryFilterTypes filter, string orderByField, Order order, int pageSize, int requestedPage);
    }
}