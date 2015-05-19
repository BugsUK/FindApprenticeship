namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using System;
    using Constants.ViewModels;
    using FluentValidation;
    using Helpers;
    using ViewModels.Candidate;

    // TODO: AG: US786: unit test.
    public class TrainingHistoryViewModelValidator : AbstractValidator<TrainingHistoryViewModel>
    {
        public TrainingHistoryViewModelValidator()
        {
            RuleFor(x => x.Provider)
                .Length(0, 50)
                .WithMessage(TrainingHistoryViewModelMessages.ProviderMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(TrainingHistoryViewModelMessages.ProviderMessages.RequiredErrorText)
                .Matches(TrainingHistoryViewModelMessages.ProviderMessages.WhiteListRegularExpression)
                .WithMessage(TrainingHistoryViewModelMessages.ProviderMessages.WhiteListErrorText);

            RuleFor(x => x.CourseTitle)
                .Length(0, 50)
                .WithMessage(TrainingHistoryViewModelMessages.CourseTitleMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(TrainingHistoryViewModelMessages.CourseTitleMessages.RequiredErrorText)
                .Matches(TrainingHistoryViewModelMessages.CourseTitleMessages.WhiteListRegularExpression)
                .WithMessage(TrainingHistoryViewModelMessages.CourseTitleMessages.WhiteListErrorText);

            var maxYear = Convert.ToString(DateTime.Now.Year - 100);
            RuleFor(x => x.FromYear)
                .NotEmpty()
                .WithMessage(TrainingHistoryViewModelMessages.FromYearMessages.RequiredErrorText)
                .Matches(TrainingHistoryViewModelMessages.FromYearMessages.WhiteListRegularExpression)
                .WithMessage(TrainingHistoryViewModelMessages.FromYearMessages.WhiteListErrorText)
                .Must(ValidatorsHelper.BeNowOrInThePast)
                .WithMessage(TrainingHistoryViewModelMessages.FromYearMessages.CanNotBeInTheFutureErrorText)
                .GreaterThanOrEqualTo(maxYear)
                .WithMessage(TrainingHistoryViewModelMessages.FromYearMessages.MustBeGreaterThan(maxYear));

            RuleFor(x => x.ToYear)
                .Matches(TrainingHistoryViewModelMessages.ToYearMessages.WhiteListRegularExpression)
                .WithMessage(TrainingHistoryViewModelMessages.ToYearMessages.WhiteListErrorText)
                .Must(ValidatorsHelper.BeNowOrInThePast)
                .WithMessage(TrainingHistoryViewModelMessages.ToYearMessages.CanNotBeInTheFutureErrorText)
                .GreaterThanOrEqualTo(maxYear)
                .WithMessage(TrainingHistoryViewModelMessages.FromYearMessages.MustBeGreaterThan(maxYear))
                .Must(ValidatorsHelper.TrainingYearBeBeforeOrEqual)
                .WithMessage(TrainingHistoryViewModelMessages.FromYearMessages.BeforeOrEqualErrorText);
        }
    }
}