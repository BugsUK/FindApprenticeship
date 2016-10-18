namespace SFA.Apprenticeships.Web.Raa.Common.Validators.VacancyPosting
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.VacancyPosting;

    public class LocationSearchViewModelClientValidator : AbstractValidator<LocationSearchViewModel>
    {
        public LocationSearchViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class LocationSearchViewModelServerValidator : AbstractValidator<LocationSearchViewModel>
    {
        public LocationSearchViewModelServerValidator()
        {
            this.AddCommonRules();

            RuleFor(m => m.Addresses)
                .NotEmpty()
                .WithMessage(LocationSearchViewModelMessages.Addresses.NoAddressesErrorText);

            RuleFor(m => m.Addresses)
                .SetCollectionValidator(new VacancyLocationAddressViewModelClientValidator());
        }
    }

    internal static class LocationSearchViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<LocationSearchViewModel> validator)
        {
            validator.RuleFor(viewModel => viewModel.AdditionalLocationInformation)
                .Matches(LocationSearchViewModelMessages.AdditionalLocationInformation.WhiteListRegularExpression)
                .WithMessage(LocationSearchViewModelMessages.AdditionalLocationInformation.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.AdditionalLocationInformationComment)
                .Matches(LocationSearchViewModelMessages.AdditionalLocationInformationComment.WhiteListRegularExpression)
                .WithMessage(LocationSearchViewModelMessages.AdditionalLocationInformationComment.WhiteListErrorText);
        }
    }
}