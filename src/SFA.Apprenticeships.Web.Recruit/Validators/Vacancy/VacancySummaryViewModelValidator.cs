namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using System;
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Vacancy;

    public class VacancySummaryViewModelClientValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelClientValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(x => x.WorkingWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.WorkingWeek.RequiredErrorText);
            /*.Length(0, 100)
            .WithMessage(VacancyViewModelMessages.WorkingWeek.TooLongErrorText)
            .Matches(VacancyViewModelMessages.WorkingWeek.WhiteListRegularExpression)
            .WithMessage(VacancyViewModelMessages.WorkingWeek.WhiteListErrorText);*/

            RuleFor(x => x.WeeklyWage)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.WeeklyWage.RequiredErrorText);

            RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Duration.RequiredErrorText);

            RuleFor(x => x.ClosingDate)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.ClosingDate.RequiredErrorText)
                .GreaterThan(DateTime.Today)
                .WithMessage(VacancyViewModelMessages.ClosingDate.TooSoonErrorText);

            RuleFor(x => x.PossibleStartDate)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.RequiredErrorText)
                .GreaterThan(DateTime.Today)
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.TooSoonErrorText);

            RuleFor(x => x.LongDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.LongDescription.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.LongDescription.TooLongErrorText)
                .Matches(VacancyViewModelMessages.LongDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.LongDescription.WhiteListErrorText);
        }
    }

    public class VacancySummaryViewModelServerValidator : VacancySummaryViewModelClientValidator
    {
        
    }
}