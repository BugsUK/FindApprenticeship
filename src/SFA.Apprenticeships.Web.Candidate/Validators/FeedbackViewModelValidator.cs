namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Home;

    public class FeedbackClientViewModelValidator : AbstractValidator<FeedbackViewModel>
    {
        public FeedbackClientViewModelValidator()
        {
            this.AddCommonRules();
        }
    }

    public class FeedbackServerViewModelValidator : AbstractValidator<FeedbackViewModel>
    {
        public FeedbackServerViewModelValidator()
        {
            this.AddCommonRules();
        }
    }

    internal static class FeedbackValidationRules
    {
        internal static void AddCommonRules(this AbstractValidator<FeedbackViewModel> validator)
        {
            validator.RuleFor(x => x.Name)
                .Length(0, 71)
                .WithMessage(FeedbackViewModelMessages.FullNameMessages.TooLongErrorText)
                .Matches(FeedbackViewModelMessages.FullNameMessages.WhiteListRegularExpression)
                .WithMessage(FeedbackViewModelMessages.FullNameMessages.WhiteListErrorText);


            validator.RuleFor(x => x.Email)
                .Length(0, 100)
                .WithMessage(FeedbackViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .Matches(FeedbackViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(FeedbackViewModelMessages.EmailAddressMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Details)
                .NotEmpty()
                .WithMessage(FeedbackViewModelMessages.DetailsMessages.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(FeedbackViewModelMessages.DetailsMessages.TooLongErrorText)
                .Matches(FeedbackViewModelMessages.DetailsMessages.WhiteListRegularExpression)
                .WithMessage(FeedbackViewModelMessages.DetailsMessages.WhiteListErrorText);
        }
    }
}