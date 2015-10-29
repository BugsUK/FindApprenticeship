using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy;

namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    public static class VacancySummaryViewModelBusinessRulesExtensions
    {
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
                    return viewModel.Duration >= 52;
                case DurationType.Months:
                    return viewModel.Duration >= 12;
                case DurationType.Years:
                    return viewModel.Duration >= 1;
                default:
                    return false;
            }
        }

        public static bool ExpectedDurationGreaterThanOrEqualToMinimumDuration(this VacancySummaryViewModel viewModel)
        {
            if (!viewModel.HoursPerWeek.HasValue || !viewModel.Duration.HasValue)
                return false;

            var hoursAndMinDurationLookup = HoursAndMinDurationLookup;

            var condition =
                hoursAndMinDurationLookup.First(
                    l =>
                        viewModel.HoursPerWeek.Value >= l.HoursInclusiveLowerBound &&
                        viewModel.HoursPerWeek.Value < l.HoursExclusiveUpperBound);

            return condition.IsGreaterThanOrEqualToMinDuration(viewModel.Duration.Value, viewModel.DurationType);
        }
    }

    public class MinimumDurationForHoursPerWeek
    {
        public decimal? HoursInclusiveLowerBound { get; private set; }
        public decimal? HoursExclusiveUpperBound { get; private set; }
        public decimal MinimumDurationInMonths { get; private set; }
        public string WarningMessage { get; private set; }

        internal MinimumDurationForHoursPerWeek(decimal hoursLowerBound, decimal? hoursUpperBound,
            decimal minimumDurationInMonths, string warningMessage)
        {
            HoursInclusiveLowerBound = hoursLowerBound;
            HoursExclusiveUpperBound = hoursUpperBound;
            MinimumDurationInMonths = minimumDurationInMonths;
            WarningMessage = warningMessage;
        }

        private MinimumDurationForHoursPerWeek() {}

        public bool IsGreaterThanOrEqualToMinDuration(int duration, DurationType durationType)
        {
            var durationInWeeks = 0m;

            switch (durationType)
            {
                case DurationType.Weeks:
                    durationInWeeks = duration;
                    break;
                case DurationType.Months:
                    durationInWeeks = duration * 52/12; //TODO: extract conversion into extension method on decimal? and other types
                    break;
                case DurationType.Years:
                    durationInWeeks = duration * 52;
                    break;
                default:
                    return false;
            }

            var minimumDurationInWeeks = MinimumDurationInMonths * 52/12;

            return durationInWeeks >= minimumDurationInWeeks;
        }
    }
}