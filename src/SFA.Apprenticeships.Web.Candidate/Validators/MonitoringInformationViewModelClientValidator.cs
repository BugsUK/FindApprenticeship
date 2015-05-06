namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Candidate;

    public class MonitoringInformationViewModelValidator : AbstractValidator<MonitoringInformationViewModel>
    {
        public MonitoringInformationViewModelValidator()
        {
            this.AddCommonRules();
        }
    }

    internal static class MonitoringInformationValidationRules
    {
        public static void AddCommonRules(this AbstractValidator<MonitoringInformationViewModel> validator)
        {
            validator
                .RuleFor(x => x.AnythingWeCanDoToSupportYourInterview)
                .NotEmpty()
                .WithMessage(MonitoringInformationViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(MonitoringInformationViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.TooLongErrorText)
                .Matches(MonitoringInformationViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListRegularExpression)
                .WithMessage(MonitoringInformationViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.WhiteListErrorText)
                .When(x => x.RequiresSupportForInterview);
        }
    }
}