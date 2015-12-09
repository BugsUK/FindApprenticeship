namespace SFA.Apprenticeships.Web.Recruit.Validators.VacancyPosting
{
    using FluentValidation;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.ViewModels.VacancyPosting;

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