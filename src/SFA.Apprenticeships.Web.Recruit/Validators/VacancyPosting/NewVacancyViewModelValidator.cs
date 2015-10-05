namespace SFA.Apprenticeships.Web.Recruit.Validators.VacancyPosting
{
    using FluentValidation;
    using ViewModels.Vacancy;

    public class NewVacancyViewModelValidator : AbstractValidator<NewVacancyViewModel>
    {
        public NewVacancyViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            throw new System.NotImplementedException();
        }
    }
}
