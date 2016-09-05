namespace SFA.Apprenticeships.Web.Raa.Common.Extensions
{
    using System;
    using System.Linq;
    using Infrastructure.Presentation.Constants;
    using Infrastructure.Presentation.Entities;
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