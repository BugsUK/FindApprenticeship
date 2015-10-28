namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;

    public class VacancySummaryViewModelClientValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelClientValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {

            RuleFor(viewModel => viewModel.WorkingWeek)
                .Matches(VacancyViewModelMessages.WorkingWeek.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.WhiteListErrorText);

            /*RuleFor(viewModel => viewModel.Wage)
                .Matches(VacancyViewModelMessages.WeeklyWage.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.WeeklyWage.WhiteListErrorText);

            RuleFor(viewModel => viewModel.Duration)
                .Matches(VacancyViewModelMessages.Duration.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Duration.WhiteListErrorText);*/

            RuleFor(viewModel => viewModel.LongDescription)
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

            RuleFor(x => x.WorkingWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.WorkingWeek.RequiredErrorText)
                .Matches(VacancyViewModelMessages.WorkingWeek.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.WhiteListErrorText);
            /*.Length(0, 100)
            .WithMessage(VacancyViewModelMessages.WorkingWeek.TooLongErrorText)
            .Matches(VacancyViewModelMessages.WorkingWeek.WhiteListRegularExpression)
            .WithMessage(VacancyViewModelMessages.WorkingWeek.WhiteListErrorText);*/

            RuleFor(x => x.HoursPerWeek)
                .InclusiveBetween(16, 40);

            /*RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Duration.RequiredErrorText)
                .Matches(VacancyViewModelMessages.Duration.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Duration.WhiteListErrorText);*/

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
}