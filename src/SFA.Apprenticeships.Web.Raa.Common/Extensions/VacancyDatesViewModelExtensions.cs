namespace SFA.Apprenticeships.Web.Raa.Common.Extensions
{
    using System;
    using System.Linq;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Raa.Vacancies.Constants;
    using ViewModels.Vacancy;

    public static class VacancyDatesViewModelExtensions
    {
        public static WageRange GetWageRangeForPossibleStartDate(this VacancyDatesViewModel vacancyDatesViewModel, out DateTime possibleStartDate)
        {
            var wageRange = Wages.Ranges.Last();
            possibleStartDate = wageRange.ValidFrom;
            if (vacancyDatesViewModel != null && vacancyDatesViewModel.PossibleStartDate.HasValue)
            {
                possibleStartDate = vacancyDatesViewModel.PossibleStartDate.Date;
                wageRange = Wages.GetWageRangeFor(possibleStartDate);
            }
            return wageRange;
        }
    }
}