using FluentValidation;
using SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Provider
{
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