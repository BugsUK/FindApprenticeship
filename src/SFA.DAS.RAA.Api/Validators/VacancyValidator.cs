namespace SFA.DAS.RAA.Api.Validators
{
    using FluentValidation;
    using Models;

    public class VacancyValidator : AbstractValidator<VacancyApiModel>
    {
        public VacancyValidator()
        {
            RuleFor(v => v.Title).NotEmpty().WithMessage("Please enter a title for the vacancy");
            RuleFor(v => v.ShortDescription).NotEmpty().WithMessage("Please enter a short description for the vacancy");
        }
    }
}