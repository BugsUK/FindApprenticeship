namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Extensions;
    using ViewModels.Vacancy;

    public static class VacancySummaryViewModelBusinessRulesExtensions
    {
        // TODO: we are using these constants in the decimal extension methods class. Maybe we should move it to a common place?
        private const int AYearInWeeks = 52;
        private const int AYearInMonths = 12;

        private static readonly List<MinimumDurationForHoursPerWeek> HoursAndMinDurationLookup = new List
            <MinimumDurationForHoursPerWeek>
        {
            new MinimumDurationForHoursPerWeek(16, 18, 22.5m,
                "The minimum duration is 23 months based on the hours per week selected"),
            new MinimumDurationForHoursPerWeek(18, 20, 20,
                "The minimum duration is 20 months based on the hours per week selected"),
            new MinimumDurationForHoursPerWeek(20, 22, 18,
                "The minimum duration is 18 months based on the hours per week selected"),
            new MinimumDurationForHoursPerWeek(22, 25, 16.5m,
                "The minimum duration is 17 months based on the hours per week selected"),
            new MinimumDurationForHoursPerWeek(25, 28, 14.5m,
                "The minimum duration is 15 months based on the hours per week selected"),
            new MinimumDurationForHoursPerWeek(28, 30, 13,
                "The minimum duration is 13 months based on the hours per week selected"),
            new MinimumDurationForHoursPerWeek(30, null, 12,
                "The minimum duration is 12 months based on the hours per week selected")
        };

        public static bool HoursPerWeekBetween30And40(this VacancySummaryViewModel viewModel)
        {
            return viewModel.HoursPerWeek.HasValue && viewModel.HoursPerWeek >= 30 && viewModel.HoursPerWeek <= 40;
        }

        public static bool HoursPerWeekGreaterThanOrEqualTo16(this VacancySummaryViewModel viewModel)
        {
            return viewModel.HoursPerWeek.HasValue && viewModel.HoursPerWeek >= 16;
        }

        public static bool DurationGreaterThanOrEqualTo12Months(this VacancySummaryViewModel viewModel)
        {
            switch (viewModel.DurationType)
            {
                case DurationType.Weeks:
                    return viewModel.Duration >= AYearInWeeks;
                case DurationType.Months:
                    return viewModel.Duration >= AYearInMonths;
                case DurationType.Years:
                    return viewModel.Duration >= 1;
                default:
                    return false;
            }
        }

        public static bool ExpectedDurationGreaterThanOrEqualToMinimumDuration(this VacancySummaryViewModel viewModel,
            int? duration)
        {
            if (!viewModel.HoursPerWeek.HasValue || !duration.HasValue)
                return false;

            var hoursAndMinDurationLookup = HoursAndMinDurationLookup;

            var condition =
                hoursAndMinDurationLookup.First(
                    l =>
                        viewModel.HoursPerWeek.Value >= l.HoursInclusiveLowerBound &&
                        viewModel.HoursPerWeek.Value < l.HoursExclusiveUpperBound);

            return condition.IsGreaterThanOrEqualToMinDuration(duration.Value, viewModel.DurationType);
        }
    }

    public class MinimumDurationForHoursPerWeek
    {
        internal MinimumDurationForHoursPerWeek(decimal hoursLowerBound, decimal? hoursUpperBound,
            decimal minimumDurationInMonths, string warningMessage)
        {
            HoursInclusiveLowerBound = hoursLowerBound;
            HoursExclusiveUpperBound = hoursUpperBound;
            MinimumDurationInMonths = minimumDurationInMonths;
            WarningMessage = warningMessage;
        }

        private MinimumDurationForHoursPerWeek()
        {
        }

        public decimal? HoursInclusiveLowerBound { get; }
        public decimal? HoursExclusiveUpperBound { get; }
        public decimal MinimumDurationInMonths { get; }
        public string WarningMessage { get; private set; }

        public bool IsGreaterThanOrEqualToMinDuration(decimal duration, DurationType durationType)
        {
            var durationInWeeks = 0m;

            switch (durationType)
            {
                case DurationType.Weeks:
                    durationInWeeks = duration;
                    break;
                case DurationType.Months:
                    durationInWeeks = duration.MonthsToWeeks();
                    break;
                case DurationType.Years:
                    durationInWeeks = duration.YearsToWeeks();
                    break;
                default:
                    return false;
            }

            var minimumDurationInWeeks = MinimumDurationInMonths.MonthsToWeeks();

            return durationInWeeks >= minimumDurationInWeeks;
        }
    }
}