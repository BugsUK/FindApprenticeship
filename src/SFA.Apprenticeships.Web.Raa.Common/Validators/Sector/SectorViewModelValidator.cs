namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Sector
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Admin;

    public class SectorViewModelClientValidator : AbstractValidator<SectorViewModel>
    {
        public SectorViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class SectorViewModelServerValidator : AbstractValidator<SectorViewModel>
    {
        public SectorViewModelServerValidator()
        {
            this.AddCommonRules();
        }
    }

    internal static class SectorViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<SectorViewModel> validator)
        {
            validator.RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage(SectorViewModelMessages.Name.RequiredErrorText)
                .Matches(SectorViewModelMessages.Name.WhiteListRegularExpression)
                .WithMessage(SectorViewModelMessages.Name.WhiteListErrorText);
        }
    }
}