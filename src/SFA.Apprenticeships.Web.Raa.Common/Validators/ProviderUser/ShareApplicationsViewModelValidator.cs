namespace SFA.Apprenticeships.Web.Raa.Common.Validators.ProviderUser
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Application;

    public class ShareApplicationsViewModelValidator : AbstractValidator<ShareApplicationsViewModel>
    {
        public ShareApplicationsViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(m => m.RecipientEmailAddress)
                .Length(0, 100)
                .WithMessage(ShareApplicationsViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(ShareApplicationsViewModelMessages.EmailAddressMessages.RequiredErrorText)
                .Matches(ShareApplicationsViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(ShareApplicationsViewModelMessages.EmailAddressMessages.WhiteListErrorText);
        }
    }
}
