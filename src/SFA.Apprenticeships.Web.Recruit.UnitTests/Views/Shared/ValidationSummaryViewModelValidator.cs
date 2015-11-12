namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.Shared
{
    using Common.Validators;
    using FluentValidation;

    public class ValidationSummaryViewModelValidator : AbstractValidator<ValidationSummaryViewModel>
    {
        public ValidationSummaryViewModelValidator()
        {
            RuleFor(vm => vm.ErrorIfZero)
                .NotEqual(0);

            RuleFor(vm => vm.ErrorIfNull)
                .NotEmpty();

            RuleFor(vm => vm.WarningIfZero)
                .NotEqual(0)
                .WithState(s => ValidationType.Warning);

            RuleFor(vm => vm.WarningIfNull)
                .NotEmpty()
                .WithState(s => ValidationType.Warning);
        }
    }
}