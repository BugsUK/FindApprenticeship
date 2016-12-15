namespace SFA.DAS.RAA.Api.Validators
{
    using FluentValidation;
    using Models;

    public class WageUpdateValidator : AbstractValidator<WageUpdate>
    {
        public WageUpdateValidator()
        {
            RuleSet("CompareWithExisting", () =>
            {
                RuleFor(w => w.Amount)
                .GreaterThan(wu => wu.ExistingAmount)
                .WithMessage("Amount must be greater than the existing amount.");
            });

            
            //RuleFor(v => v.ShortDescription).NotEmpty().WithMessage("Please enter a short description for the vacancy");
        }
    }
}