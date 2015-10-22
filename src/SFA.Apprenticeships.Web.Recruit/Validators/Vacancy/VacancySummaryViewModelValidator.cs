namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using System;
    using Common.Validators;
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

            RuleFor(x => x.ClosingDate).SetValidator(new DateViewModelClientValidator());

            RuleFor(x => x.PossibleStartDate).SetValidator(new DateViewModelClientValidator());

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
        public VacancySummaryViewModelServerValidator()
        {
            AddServerCommonRules();
        }

        private void AddServerCommonRules()
        {
            RuleFor(x => x.ClosingDate).SetValidator(new DateViewModelServerValidator());
            RuleFor(x => x.PossibleStartDate).SetValidator(new DateViewModelServerValidator());
        }
    }
}