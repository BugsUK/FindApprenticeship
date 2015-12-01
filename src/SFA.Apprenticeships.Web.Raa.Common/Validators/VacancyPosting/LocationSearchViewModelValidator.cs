namespace SFA.Apprenticeships.Web.Raa.Common.Validators.VacancyPosting
{
    using FluentValidation;
    using Recruit.Validators.VacancyPosting;
    using ViewModels.VacancyPosting;

    public class LocationSearchViewModelValidator : AbstractValidator<LocationSearchViewModel>
    {
        public LocationSearchViewModelValidator()
        {
            RuleFor(m => m.Addresses)
                .SetCollectionValidator(new VacancyLocationAddressViewModelValidator());
        }
    }
}