namespace SFA.Apprenticeships.Web.Raa.Common.Extensions
{
    using System.Linq;
    using Infrastructure.Presentation.Constants;
    using Infrastructure.Presentation.Entities;
    using ViewModels.Vacancy;

    public static class VacancyDatesViewModelExtensions
    {
        public static WageRange GetWageRangeForPossibleStartDate(this VacancyDatesViewModel vacancyDatesViewModel)
        {
            var wageRange = Wages.Ranges.Last();
            if (vacancyDatesViewModel != null && vacancyDatesViewModel.PossibleStartDate.HasValue)
            {
                var possibleStartDate = vacancyDatesViewModel.PossibleStartDate.Date;
                wageRange = Wages.GetWageRangeFor(possibleStartDate);
            }
            return wageRange;
        }
    }
}