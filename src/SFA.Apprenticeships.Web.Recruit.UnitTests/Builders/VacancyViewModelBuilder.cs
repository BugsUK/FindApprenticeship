namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Builders
{
    using System;
    using Common.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Ploeh.AutoFixture;
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

        public VacancyViewModel BuildValid(ProviderVacancyStatuses status)
        {
            var viewModel = new Fixture().Build<VacancyViewModel>().Create();
            viewModel.NewVacancyViewModel.TrainingType = TrainingType.Frameworks;
            viewModel.NewVacancyViewModel.OfflineVacancy = false;
            viewModel.NewVacancyViewModel.ApprenticeshipLevel = ApprenticeshipLevel.Higher;
            viewModel.VacancySummaryViewModel.Status = status;
            viewModel.VacancySummaryViewModel.HoursPerWeek = 30;
            viewModel.VacancySummaryViewModel.Duration = 12;
            viewModel.VacancySummaryViewModel.DurationType = DurationType.Months;
            viewModel.VacancySummaryViewModel.WageType = WageType.NationalMinimumWage;
            viewModel.VacancySummaryViewModel.PossibleStartDate = new DateViewModel(DateTime.UtcNow.AddDays(14));
            viewModel.VacancySummaryViewModel.ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(28));
            viewModel.Status = status;
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