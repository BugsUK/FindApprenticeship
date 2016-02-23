namespace SFA.Apprenticeships.Web.Raa.Common.Validators.VacancyPosting
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.VacancyPosting;

    public class LocationSearchViewModelValidator : AbstractValidator<LocationSearchViewModel>
    {
        public LocationSearchViewModelValidator()
        {
            RuleFor(m => m.Addresses)
                .NotEmpty()
                .WithMessage(LocationSearchViewModelMessages.Addresses.NoAddressesErrorText);

            RuleFor(m => m.Addresses)
                .SetCollectionValidator(new VacancyLocationAddressViewModelValidator());
        }
    }
}