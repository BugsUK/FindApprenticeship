namespace SFA.Apprenticeships.Web.Common.Validators
{
    using System;
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels;

    public class DateViewModelClientValidator : AbstractValidator<DateViewModel>
    {
        public DateViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class DateViewModelServerValidator : AbstractValidator<DateViewModel>
    {
        public DateViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class DateValidaitonRules
    {
        internal static void AddCommonRules(this AbstractValidator<DateViewModel> validator)
        {
            validator.RuleFor(x => x.Day)
                .InclusiveBetween(1, 31)
                .WithMessage(DateViewModelMessages.DayMessages.RangeErrorText)
                .NotEmpty()
                .WithMessage(DateViewModelMessages.DayMessages.RequiredErrorText);

            validator.RuleFor(x => x.Month)
                .InclusiveBetween(1, 12)
                .WithMessage(DateViewModelMessages.MonthMessages.RangeErrorText)
                .NotEmpty()
                .WithMessage(DateViewModelMessages.MonthMessages.RequiredErrorText);

            var fromYear = DateTime.UtcNow.Year;
            var toYear = fromYear + 10;
            validator.RuleFor(x => x.Year)
                .InclusiveBetween(fromYear, toYear)
                .WithMessage(string.Format(DateViewModelMessages.YearMessages.RangeErrorText, fromYear, toYear))
                .NotEmpty()
                .WithMessage(DateViewModelMessages.YearMessages.RequiredErrorText);
        }

        internal static void AddServerRules(this AbstractValidator<DateViewModel> validator)
        {
            validator.RuleFor(x => x.Day)
                .Must(BeValidDate)
                .WithMessage(DateViewModelMessages.MustBeValidDate);
        }

        private static bool BeValidDate(DateViewModel instance, int? day)
        {
            try
            {
                var date = instance.Date;
                return date != DateTime.MinValue;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
    }
}