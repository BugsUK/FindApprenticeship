namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.Shared
{
    using Common.Validators;
    using FluentValidation;

    public class ParentViewModelValidator : AbstractValidator<ParentViewModel>
    {
        public ParentViewModelValidator()
        {
            RuleFor(vm => vm.ErrorIfZero)
                .NotEqual(0);

            RuleFor(vm => vm.WarningIfZero)
                .NotEqual(0)
                .WithState(s => ValidationType.Warning);

            RuleFor(vm => vm.Child)
                .SetValidator(new ChildViewModelValidator());
        }
    }
}