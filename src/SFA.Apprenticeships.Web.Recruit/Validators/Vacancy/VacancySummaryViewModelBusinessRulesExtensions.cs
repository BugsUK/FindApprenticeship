namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Extensions;
    using FluentValidation.Results;
    using ViewModels.Vacancy;
    using Web.Common.Validators;
    using Web.Common.ViewModels;

    public static class VacancySummaryViewModelBusinessRulesExtensions
    {
        // TODO: we are using these constants in the decimal extension methods class. Maybe we should move it to a common place?
        private const int AYearInWeeks = 52;
        private const int AYearInMonths = 12;

        private static readonly List<MinimumDurationForHoursPerWeek> HoursAndMinDurationLookup = new List
            <MinimumDurationForHoursPerWeek>
        {
            new MinimumDurationForHoursPerWeek(16, 18, 22.5m,
                VacancyViewModelMessages.Duration.DurationWarning16Hours),
            new MinimumDurationForHoursPerWeek(18, 20, 20,
                VacancyViewModelMessages.Duration.DurationWarning18Hours),
            new MinimumDurationForHoursPerWeek(20, 22, 18,
                VacancyViewModelMessages.Duration.DurationWarning20Hours),
            new MinimumDurationForHoursPerWeek(22, 25, 16.5m,
                VacancyViewModelMessages.Duration.DurationWarning22Hours),
            new MinimumDurationForHoursPerWeek(25, 28, 14.5m,
                VacancyViewModelMessages.Duration.DurationWarning25Hours),
            new MinimumDurationForHoursPerWeek(28, 30, 13,
                VacancyViewModelMessages.Duration.DurationWarning28Hours),
            new MinimumDurationForHoursPerWeek(30, null, 12,
                VacancyViewModelMessages.Duration.DurationWarning30Hours)
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

        public static ValidationFailure ExpectedDurationGreaterThanOrEqualToMinimumDuration(this VacancySummaryViewModel viewModel,
            decimal? duration)
        {
            if (!viewModel.HoursPerWeek.HasValue || !duration.HasValue)
            {
                //Other errors will superceed this one so return valid
                return null;
            }

            var hoursAndMinDurationLookup = HoursAndMinDurationLookup;

            var condition =
                hoursAndMinDurationLookup.FirstOrDefault(
                    l =>
                        viewModel.HoursPerWeek.Value >= l.HoursInclusiveLowerBound &&
                        ( viewModel.HoursPerWeek.Value < l.HoursExclusiveUpperBound || l.HoursExclusiveUpperBound == null));

            if (condition == null)
            {
                //Other errors will superceed this one so return valid
                return null;
            }

            if (!condition.IsGreaterThanOrEqualToMinDuration(duration.Value, viewModel.DurationType))
            {
                var validationFailure = new ValidationFailure("Duration", condition.WarningMessage)
                {
                    CustomState = ValidationType.Warning
                };
                return validationFailure;
            }

            return null;
        }

        public static ValidationFailure PossibleStartDateShouldBeAfterClosingDate(this VacancySummaryViewModel viewModel, DateViewModel closingDate)
        {
            if (closingDate == null || !closingDate.HasValue || viewModel.PossibleStartDate == null ||
                !viewModel.PossibleStartDate.HasValue)
            {
                return null;
            }

            if (viewModel.PossibleStartDate.Date < closingDate.Date)
            {
                var validationFailure = new ValidationFailure("PossibleStartDate", VacancyViewModelMessages.PossibleStartDate.BeforePublishDateErrorText)
                {
                    CustomState = ValidationType.Warning
                };
                return validationFailure;
            }

            return null;
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