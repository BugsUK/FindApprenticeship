namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Builders
{
    using Raa.Common.ViewModels.Vacancy;

    public class VacancyViewModelBuilder
    {
        private NewVacancyViewModel _newVacancyViewModel = new NewVacancyViewModel();
        private VacancySummaryViewModel _vacancySummaryViewModel = new VacancySummaryViewModel();
        private VacancyRequirementsProspectsViewModel _vacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel();
        private VacancyQuestionsViewModel _vacancyQuestionsViewModel = new VacancyQuestionsViewModel();

        public VacancyViewModel Build()
        {
            var viewModel = new VacancyViewModel
            {
                NewVacancyViewModel = _newVacancyViewModel,
                VacancySummaryViewModel = _vacancySummaryViewModel,
                VacancyRequirementsProspectsViewModel = _vacancyRequirementsProspectsViewModel,
                VacancyQuestionsViewModel = _vacancyQuestionsViewModel
            };
            return viewModel;
        }

        public VacancyViewModelBuilder With(NewVacancyViewModel newVacancyViewModel)
        {
            _newVacancyViewModel = newVacancyViewModel;
            return this;
        }

        public VacancyViewModelBuilder With(VacancySummaryViewModel vacancySummaryViewModel)
        {
            _vacancySummaryViewModel = vacancySummaryViewModel;
            return this;
        }

        public VacancyViewModelBuilder With(VacancyRequirementsProspectsViewModel vacancyRequirementsProspectsViewModel)
        {
            _vacancyRequirementsProspectsViewModel = vacancyRequirementsProspectsViewModel;
            return this;
        }

        public VacancyViewModelBuilder With(VacancyQuestionsViewModel vacancyQuestionsViewModel)
        {
            _vacancyQuestionsViewModel = vacancyQuestionsViewModel;
            return this;
        }
    }
}