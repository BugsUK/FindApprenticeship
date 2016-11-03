namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Builders
{
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using Ploeh.AutoFixture;
    using System;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels;
    using TrainingType = Domain.Entities.Raa.Vacancies.TrainingType;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    public class VacancyViewModelBuilder
    {
        private NewVacancyViewModel _newVacancyViewModel = new NewVacancyViewModel();
        private TrainingDetailsViewModel _trainingDetailsViewModel = new TrainingDetailsViewModel();
        private VacancyRequirementsProspectsViewModel _vacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel();
        private VacancyQuestionsViewModel _vacancyQuestionsViewModel = new VacancyQuestionsViewModel();
        private FurtherVacancyDetailsViewModel _furtherVacancyDetailsViewModel = new FurtherVacancyDetailsViewModel
        {
            VacancyDatesViewModel = new VacancyDatesViewModel()
        };

        public VacancyViewModel Build()
        {
            var viewModel = new VacancyViewModel
            {
                NewVacancyViewModel = _newVacancyViewModel,
                TrainingDetailsViewModel = _trainingDetailsViewModel,
                FurtherVacancyDetailsViewModel = _furtherVacancyDetailsViewModel,
                VacancyRequirementsProspectsViewModel = _vacancyRequirementsProspectsViewModel,
                VacancyQuestionsViewModel = _vacancyQuestionsViewModel
            };
            return viewModel;
        }

        public VacancyViewModel BuildValid(VacancyStatus status, VacancyType vacancyType)
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
            viewModel.FurtherVacancyDetailsViewModel.Status = status;
            viewModel.FurtherVacancyDetailsViewModel.VacancyType = vacancyType;
            viewModel.FurtherVacancyDetailsViewModel.Duration = 12;
            viewModel.FurtherVacancyDetailsViewModel.DurationType = DurationType.Months;
            viewModel.FurtherVacancyDetailsViewModel.Wage = new WageViewModel()
            {
                Type = WageType.NationalMinimum,
                Classification = WageClassification.NationalMinimum,
                Amount = null,
                AmountLowerBound = null,
                AmountUpperBound = null,
                Text = null,
                Unit = WageUnit.NotApplicable,
                HoursPerWeek = 30
            };
            viewModel.FurtherVacancyDetailsViewModel.VacancyDatesViewModel = new VacancyDatesViewModel
            {
                PossibleStartDate = new DateViewModel(DateTime.UtcNow.AddDays(28)),
                ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(14))
            };
            viewModel.VacancyRequirementsProspectsViewModel.VacancyType = vacancyType;
            viewModel.Status = status;
            viewModel.VacancyType = vacancyType;
            viewModel.IsCandidateView = false;
            viewModel.IsManageReviewerView = false;
            viewModel.VacancySource = VacancySource.Raa;

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

        public VacancyViewModelBuilder With(FurtherVacancyDetailsViewModel furtherVacancyDetailsViewModel)
        {
            _furtherVacancyDetailsViewModel = furtherVacancyDetailsViewModel;
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
            _furtherVacancyDetailsViewModel.VacancyDatesViewModel = vacancyDatesViewModel;
            return this;
        }
    }
}