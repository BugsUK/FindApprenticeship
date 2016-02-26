namespace SFA.Apprenticeships.Web.Common.Validators
{
    using System;
    using Constants.ViewModels;
    using FluentValidation;
    using Helpers;
    using ViewModels.Candidate;

    public class TrainingCourseViewModelValidator : AbstractValidator<TrainingCourseViewModel>
    {
        public TrainingCourseViewModelValidator()
        {
            RuleFor(x => x.Provider)
                .Length(0, 50)
                .WithMessage(TrainingCourseViewModelMessages.ProviderMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(TrainingCourseViewModelMessages.ProviderMessages.RequiredErrorText)
                .Matches(TrainingCourseViewModelMessages.ProviderMessages.WhiteListRegularExpression)
                .WithMessage(TrainingCourseViewModelMessages.ProviderMessages.WhiteListErrorText);

            RuleFor(x => x.Title)
                .Length(0, 50)
                .WithMessage(TrainingCourseViewModelMessages.TitleMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(TrainingCourseViewModelMessages.TitleMessages.RequiredErrorText)
                .Matches(TrainingCourseViewModelMessages.TitleMessages.WhiteListRegularExpression)
                .WithMessage(TrainingCourseViewModelMessages.TitleMessages.WhiteListErrorText);

            var maxYear = Convert.ToString(DateTime.UtcNow.Year - 100);

            RuleFor(x => x.FromYear)
                .NotEmpty()
                .WithMessage(TrainingCourseViewModelMessages.FromYearMessages.RequiredErrorText)
                .Matches(TrainingCourseViewModelMessages.FromYearMessages.WhiteListRegularExpression)
                .WithMessage(TrainingCourseViewModelMessages.FromYearMessages.WhiteListErrorText)
                .Must(ValidatorsHelper.BeNowOrInThePast)
                .WithMessage(TrainingCourseViewModelMessages.FromYearMessages.CanNotBeInTheFutureErrorText)
                .GreaterThanOrEqualTo(maxYear)
                .WithMessage(TrainingCourseViewModelMessages.FromYearMessages.MustBeGreaterThan(maxYear));

            RuleFor(x => x.ToYear)
                .NotEmpty()
                .WithMessage(TrainingCourseViewModelMessages.ToYearMessages.RequiredErrorText)
                .Matches(TrainingCourseViewModelMessages.ToYearMessages.WhiteListRegularExpression)
                .WithMessage(TrainingCourseViewModelMessages.ToYearMessages.WhiteListErrorText)
                .GreaterThanOrEqualTo(maxYear)
                .WithMessage(TrainingCourseViewModelMessages.FromYearMessages.MustBeGreaterThan(maxYear))
                .Must(ValidatorsHelper.TrainingYearBeBeforeOrEqual)
                .WithMessage(TrainingCourseViewModelMessages.FromYearMessages.BeforeOrEqualErrorText);
        }
    }
}