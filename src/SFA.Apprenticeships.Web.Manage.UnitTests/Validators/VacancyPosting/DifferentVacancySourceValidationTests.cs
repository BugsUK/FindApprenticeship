namespace SFA.Apprenticeships.Web.Manage.UnitTests.Validators.VacancyPosting
{
    using System;
    using Common.UnitTests.Validators;
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    internal class DifferentVacancySourceValidationTests

    {
        [SetUp]
        public void SetUp()
        {
            _aggregateValidator = new VacancyViewModelValidator();
        }

        private VacancyViewModelValidator _aggregateValidator;

        [TestCase(VacancySource.Raa, null, false)]
        [TestCase(VacancySource.Raa, "", false)]
        [TestCase(VacancySource.Raa, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Av, null, true)]
        [TestCase(VacancySource.Av, "", true)]
        [TestCase(VacancySource.Av, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Api, null, true)]
        [TestCase(VacancySource.Api, "", true)]
        [TestCase(VacancySource.Api, Samples.ValidFreeText, true)]
        public void SkillsRequiredValidation(VacancySource vacancySource, string text, bool expectValid)
        {
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.VacancyRequirementsProspectsViewModel.DesiredSkills = text;

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, null, false)]
        [TestCase(VacancySource.Raa, "", false)]
        [TestCase(VacancySource.Raa, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Av, null, true)]
        [TestCase(VacancySource.Av, "", true)]
        [TestCase(VacancySource.Av, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Api, null, true)]
        [TestCase(VacancySource.Api, "", true)]
        [TestCase(VacancySource.Api, Samples.ValidFreeText, true)]
        public void DesiredQualificationsValidation(VacancySource vacancySource, string text, bool expectValid)
        {
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.VacancyRequirementsProspectsViewModel.DesiredQualifications = text;

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, null, false)]
        [TestCase(VacancySource.Raa, "", false)]
        [TestCase(VacancySource.Raa, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Av, null, true)]
        [TestCase(VacancySource.Av, "", true)]
        [TestCase(VacancySource.Av, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Api, null, true)]
        [TestCase(VacancySource.Api, "", true)]
        [TestCase(VacancySource.Api, Samples.ValidFreeText, true)]
        public void PersonalQualitiesValidation(VacancySource vacancySource, string text, bool expectValid)
        {
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.VacancyRequirementsProspectsViewModel.PersonalQualities = text;

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, null, false)]
        [TestCase(VacancySource.Raa, "", false)]
        [TestCase(VacancySource.Raa, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Av, null, true)]
        [TestCase(VacancySource.Av, "", true)]
        [TestCase(VacancySource.Av, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Api, null, true)]
        [TestCase(VacancySource.Api, "", true)]
        [TestCase(VacancySource.Api, Samples.ValidFreeText, true)]
        public void FutureProspectsValidation(VacancySource vacancySource, string text, bool expectValid)
        {
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.VacancyRequirementsProspectsViewModel.FutureProspects = text;

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        private VacancyViewModel BuildValidVacancy(VacancySource vacancySource)
        {
            var viewModel = new Fixture().Build<VacancyViewModel>().Create();
            viewModel.NewVacancyViewModel.VacancyType = VacancyType.Apprenticeship;
            viewModel.NewVacancyViewModel.OfflineVacancy = false;
            viewModel.NewVacancyViewModel.OfflineApplicationUrl = null;
            viewModel.NewVacancyViewModel.OfflineApplicationInstructions = null;
            viewModel.TrainingDetailsViewModel.VacancyType = VacancyType.Apprenticeship;
            viewModel.TrainingDetailsViewModel.TrainingType = TrainingType.Frameworks;
            viewModel.TrainingDetailsViewModel.ApprenticeshipLevel = ApprenticeshipLevel.Higher;
            viewModel.TrainingDetailsViewModel.ContactName = null;
            viewModel.TrainingDetailsViewModel.ContactNumber = null;
            viewModel.TrainingDetailsViewModel.ContactEmail = null;
            viewModel.FurtherVacancyDetailsViewModel.Status = VacancyStatus.Live;
            viewModel.FurtherVacancyDetailsViewModel.VacancyType = VacancyType.Apprenticeship;
            viewModel.FurtherVacancyDetailsViewModel.HoursPerWeek = 30;
            viewModel.FurtherVacancyDetailsViewModel.Duration = 12;
            viewModel.FurtherVacancyDetailsViewModel.DurationType = DurationType.Months;
            viewModel.FurtherVacancyDetailsViewModel.WageType = WageType.NationalMinimum;
            viewModel.FurtherVacancyDetailsViewModel.VacancyDatesViewModel = new VacancyDatesViewModel
            {
                PossibleStartDate = new DateViewModel(DateTime.UtcNow.AddDays(28)),
                ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(14))
            };
            viewModel.VacancyRequirementsProspectsViewModel.VacancyType = VacancyType.Apprenticeship;
            viewModel.Status = VacancyStatus.Live;
            viewModel.VacancyType = VacancyType.Apprenticeship;
            viewModel.VacancySource = vacancySource;
            viewModel.VacancyRequirementsProspectsViewModel.VacancySource = vacancySource;

            return viewModel;
        }
    }
}