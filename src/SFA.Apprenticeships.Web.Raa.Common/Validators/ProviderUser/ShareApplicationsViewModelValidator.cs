namespace SFA.Apprenticeships.Web.Raa.Common.Validators.ProviderUser
{
    using Constants.ViewModels;
    using FluentValidation;
    using System.Linq;
    using ViewModels.Application;

    public class ShareApplicationsViewModelValidator : AbstractValidator<ShareApplicationsViewModel>
    {
        public ShareApplicationsViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(m => m.SelectedApplicationIds)
                .Must(sa => sa.Any())
                .WithMessage(ShareApplicationsViewModelMessages.SelectedApplicationsMessages.RequiredErrorText);

            RuleFor(m => m.RecipientEmailAddress)
                .Length(0, 100)
                .WithMessage(ShareApplicationsViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(ShareApplicationsViewModelMessages.EmailAddressMessages.RequiredErrorText)
                .Matches(ShareApplicationsViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(ShareApplicationsViewModelMessages.EmailAddressMessages.WhiteListErrorText);

            RuleFor(m => m.OptionalMessage)
                .Length(0, 350)
                .WithMessage(ShareApplicationsViewModelMessages.OptionalMessage.TooLongErrorText)
                .Matches(ShareApplicationsViewModelMessages.OptionalMessage.WhiteListRegularExpression)
                .WithMessage(ShareApplicationsViewModelMessages.OptionalMessage.WhiteListErrorText);
        }
    }
}
