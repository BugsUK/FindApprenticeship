namespace SFA.Apprenticeships.Web.Manage.Validators.Provider
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Provider;

    public class ProviderViewModelValidator : AbstractValidator<ProviderViewModel>
    {
        public ProviderViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(m => m.ProviderName)
                .NotEmpty()
                .WithMessage(ProviderViewModelMessages.ProviderNameMessages.RequiredErrorText)
                .Length(0, 100)
                .WithMessage(ProviderViewModelMessages.ProviderNameMessages.TooLongErrorText);
        }
    }
}