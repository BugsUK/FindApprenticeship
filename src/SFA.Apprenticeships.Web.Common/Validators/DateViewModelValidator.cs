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
                .NotEmpty()
                .WithMessage(DateViewModelMessages.DayMessages.RequiredErrorText)
                .InclusiveBetween(1, 31)
                .WithMessage(DateViewModelMessages.DayMessages.RangeErrorText);

            validator.RuleFor(x => x.Month)
                .NotEmpty()
                .WithMessage(DateViewModelMessages.MonthMessages.RequiredErrorText)
                .InclusiveBetween(1, 12)
                .WithMessage(DateViewModelMessages.MonthMessages.RangeErrorText);

            var fromYear = DateTime.UtcNow.Year;
            var toYear = fromYear + 10;
            validator.RuleFor(x => x.Year)
                .NotEmpty()
                .WithMessage(DateViewModelMessages.YearMessages.RequiredErrorText)
                .InclusiveBetween(fromYear, toYear)
                .WithMessage(string.Format(DateViewModelMessages.YearMessages.RangeErrorText, fromYear, toYear));
        }

        internal static void AddServerRules(this AbstractValidator<DateViewModel> validator)
        {
            
        }
    }
}