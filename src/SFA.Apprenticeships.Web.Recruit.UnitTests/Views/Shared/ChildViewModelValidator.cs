namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.Shared
{
    using Common.Validators;
    using FluentValidation;

    public class ChildViewModelValidator : AbstractValidator<ChildViewModel>
    {
        public ChildViewModelValidator()
        {
            RuleFor(vm => vm.ErrorIfNull)
                .NotEmpty();

            RuleFor(vm => vm.WarningIfNull)
                .NotEmpty()
                .WithState(s => ValidationType.Warning);
        }
    }
}