namespace SFA.Apprenticeships.Web.Raa.Common.Validators.VacancyPosting
{
    using FluentValidation;
    using Constants.ViewModels;
    using ViewModels.VacancyPosting;

    public class VacancyLocationAddressViewModelValidator : AbstractValidator<VacancyLocationAddressViewModel>
    {
        public VacancyLocationAddressViewModelValidator()
        {
            RuleFor(x => x.NumberOfPositions)
                .NotEmpty()
                .WithMessage(VacancyLocationAddressViewModelMessages.NumberOfPositions.RequiredErrorText)
                .GreaterThanOrEqualTo(1)
                .WithMessage(VacancyLocationAddressViewModelMessages.NumberOfPositions.AtLeastOnePositionErrorText);
        }
    }
}