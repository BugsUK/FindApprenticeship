namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Standard
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Admin;

    public class StandardViewModelClientValidator : AbstractValidator<StandardViewModel>
    {
        public StandardViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class StandardViewModelServerValidator : AbstractValidator<StandardViewModel>
    {
        public StandardViewModelServerValidator()
        {
            this.AddCommonRules();
        }
    }

    internal static class StandardViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<StandardViewModel> validator)
        {
            validator.RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage(StandardViewModelMessages.Name.RequiredErrorText)
                .Matches(StandardViewModelMessages.Name.WhiteListRegularExpression)
                .WithMessage(StandardViewModelMessages.Name.WhiteListErrorText);
        }
    }
}