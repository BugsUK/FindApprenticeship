namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Builders
{
    using System;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Ploeh.AutoFixture;
    using Raa.Common.ViewModels.Vacancy;
    using Common.ViewModels;

    public class VacancyViewModelBuilder
    {
        private NewVacancyViewModel _newVacancyViewModel = new NewVacancyViewModel();
        private TrainingDetailsViewModel _trainingDetailsViewModel = new TrainingDetailsViewModel();
        private VacancyRequirementsProspectsViewModel _vacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel();
        private VacancyQuestionsViewModel _vacancyQuestionsViewModel = new VacancyQuestionsViewModel();
        private VacancySummaryViewModel _vacancySummaryViewModel = new VacancySummaryViewModel
        {
            VacancyDatesViewModel = new VacancyDatesViewModel()
        };

        public VacancyViewModel Build()
        {
            var viewModel = new VacancyViewModel
            {
                NewVacancyViewModel = _newVacancyViewModel,
                TrainingDetailsViewModel = _trainingDetailsViewModel,
                VacancySummaryViewModel = _vacancySummaryViewModel,
                VacancyRequirementsProspectsViewModel = _vacancyRequirementsProspectsViewModel,
                VacancyQuestionsViewModel = _vacancyQuestionsViewModel
            };
            return viewModel;
        }

        public VacancyViewModel BuildValid(ProviderVacancyStatuses status, VacancyType vacancyType)
        {
            var viewModel = new Fixture().Build<VacancyViewModel>().Create();
            viewModel.NewVacancyViewModel.VacancyType = vacancyType;
            viewModel.NewVacancyViewModel.OfflineVacancy = false;
            viewModel.NewVacancyViewModel.OfflineApplicationUrl = null;
            viewModel.NewVacancyViewModel.OfflineApplicationInstructions = null;
            viewModel.TrainingDetailsViewModel.VacancyType = vacancyType;
            viewModel.TrainingDetailsViewModel.TrainingType = TrainingType.Frameworks;
            viewModel.TrainingDetailsViewModel.ApprenticeshipLevel = ApprenticeshipLevel.Higher;
            viewModel.TrainingDetailsViewModel.ContactName = null;
            viewModel.TrainingDetailsViewModel.ContactNumber = null;
            viewModel.TrainingDetailsViewModel.ContactEmail = null;
            viewModel.VacancySummaryViewModel.Status = status;
            viewModel.VacancySummaryViewModel.VacancyType = vacancyType;
            viewModel.VacancySummaryViewModel.HoursPerWeek = 30;
            viewModel.VacancySummaryViewModel.Duration = 12;
            viewModel.VacancySummaryViewModel.DurationType = DurationType.Months;
            viewModel.VacancySummaryViewModel.WageType = WageType.NationalMinimumWage;
            viewModel.VacancySummaryViewModel.VacancyDatesViewModel = new VacancyDatesViewModel
            {
                PossibleStartDate = new DateViewModel(DateTime.UtcNow.AddDays(28)),
                ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(14))
            };
            viewModel.VacancyRequirementsProspectsViewModel.VacancyType = vacancyType;
            viewModel.Status = status;
            viewModel.VacancyType = vacancyType;
            return viewModel;
        }

        public VacancyViewModelBuilder With(NewVacancyViewModel newVacancyViewModel)
        {
            _newVacancyViewModel = newVacancyViewModel;
            return this;
        }

        public VacancyViewModelBuilder With(TrainingDetailsViewModel trainingDetailsViewModel)
        {
            _trainingDetailsViewModel = trainingDetailsViewModel;
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

        public VacancyViewModelBuilder With(VacancyDatesViewModel vacancyDatesViewModel)
        {
            _vacancySummaryViewModel.VacancyDatesViewModel = vacancyDatesViewModel;
            return this;
        }
    }
}