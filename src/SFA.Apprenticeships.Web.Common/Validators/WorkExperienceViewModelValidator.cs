namespace SFA.Apprenticeships.Web.Common.Validators
{
    using System;
    using Helpers;
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Candidate;

    public class WorkExperienceViewModelValidator : AbstractValidator<WorkExperienceViewModel>
    {
        public WorkExperienceViewModelValidator()
        {
            RuleFor(x => x.Description)
                .Length(0, 200)
                .WithMessage(WorkExperienceViewModelMessages.DescriptionMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(WorkExperienceViewModelMessages.DescriptionMessages.RequiredErrorText)
                .Matches(WorkExperienceViewModelMessages.DescriptionMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.DescriptionMessages.WhiteListErrorText);

            RuleFor(x => x.Employer)
                .Length(0, 50)
                .WithMessage(WorkExperienceViewModelMessages.EmployerMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(WorkExperienceViewModelMessages.EmployerMessages.RequiredErrorText)
                .Matches(WorkExperienceViewModelMessages.EmployerMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.EmployerMessages.WhiteListErrorText);

            RuleFor(x => x.JobTitle)
                .Length(0, 50)
                .WithMessage(WorkExperienceViewModelMessages.JobTitleMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(WorkExperienceViewModelMessages.JobTitleMessages.RequiredErrorText)
                .Matches(WorkExperienceViewModelMessages.JobTitleMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.JobTitleMessages.WhiteListErrorText);

            var maxYear = Convert.ToString(DateTime.UtcNow.Year - 100);

            RuleFor(x => x.FromYear)
                .NotEmpty()
                .WithMessage(WorkExperienceViewModelMessages.FromYearMessages.RequiredErrorText)
                .Matches(WorkExperienceViewModelMessages.FromYearMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.FromYearMessages.WhiteListErrorText)
                .Must(ValidatorsHelper.BeNowOrInThePast)
                .WithMessage(WorkExperienceViewModelMessages.FromYearMessages.CanNotBeInTheFutureErrorText)
                .GreaterThanOrEqualTo(maxYear)
                .WithMessage(WorkExperienceViewModelMessages.FromYearMessages.MustBeGreaterThan(maxYear));

            RuleFor(x => x.ToYear)
                .Matches(WorkExperienceViewModelMessages.ToYearMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.ToYearMessages.WhiteListErrorText)
                .Must(ValidatorsHelper.BeNowOrInThePast)
                .WithMessage(WorkExperienceViewModelMessages.ToYearMessages.CanNotBeInTheFutureErrorText)
                .GreaterThanOrEqualTo(maxYear)
                .WithMessage(WorkExperienceViewModelMessages.FromYearMessages.MustBeGreaterThan(maxYear))
                .Must(ValidatorsHelper.WorkExperienceYearBeBeforeOrEqual)
                .WithMessage(WorkExperienceViewModelMessages.FromYearMessages.BeforeOrEqualErrorText);
        }
    }
}