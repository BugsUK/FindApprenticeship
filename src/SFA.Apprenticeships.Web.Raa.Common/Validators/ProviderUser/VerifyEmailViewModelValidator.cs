using SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser;

namespace SFA.Apprenticeships.Web.Recruit.Validators.ProviderUser
{
    using FluentValidation;

    public class VerifyEmailViewModelValidator : AbstractValidator<VerifyEmailViewModel>
    {
        public VerifyEmailViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(m => m.VerificationCode)
                .NotEmpty()
                .WithMessage(VerifyEmailViewModelMessages.VerificationCode.RequiredErrorText)
                .Length(6, 6)
                .WithMessage(VerifyEmailViewModelMessages.VerificationCode.LengthErrorText)
                .Matches(VerifyEmailViewModelMessages.VerificationCode.WhiteListRegularExpression)
                .WithMessage(VerifyEmailViewModelMessages.VerificationCode.WhiteListErrorText);
        }
    }
}