namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using System;
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels;

    public class DateOfBirthViewModelClientValidator : AbstractValidator<DateOfBirthViewModel>
    {
        public DateOfBirthViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class DateOfBirthViewModelServerValidator : AbstractValidator<DateOfBirthViewModel>
    {
        public DateOfBirthViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class DateOfBirthValidaitonRules
    {
        internal static void AddCommonRules(this AbstractValidator<DateOfBirthViewModel> validator)
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

            validator.RuleFor(x => x.Year)
                .InclusiveBetween(DateTime.UtcNow.Year - 100, DateTime.UtcNow.Year)
                .WithMessage(string.Format(DateViewModelMessages.YearMessages.RangeErrorText, DateTime.UtcNow.Year - 100, DateTime.UtcNow.Year))
                .NotEmpty()
                .WithMessage(DateViewModelMessages.YearMessages.RequiredErrorText);
        }

        internal static void AddServerRules(this AbstractValidator<DateOfBirthViewModel> validator)
        {
            validator.RuleFor(x => x.Day)
                .Must(BeValidDate)
                .WithMessage(DateViewModelMessages.MustBeValidDate);
        }

        private static bool BeValidDate(DateOfBirthViewModel instance, int? day)
        {
            if (!instance.Year.HasValue || !instance.Month.HasValue || !instance.Day.HasValue)
            {
                return false;
            }

            try
            {
                var date = new DateTime(instance.Year.Value, instance.Month.Value, instance.Day.Value);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            return true;
        }
    }
}