namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Common.UnitTests.Validators;
    using Common.Validators;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.UnitTests.Builders;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class NewVacancyViewModelServerValidatorTests
    {
        private NewVacancyViewModelServerValidator _validator;
        private VacancyViewModelValidator _aggregateValidator;

        [SetUp]
        public void SetUp()
        {
            _validator = new NewVacancyViewModelServerValidator();
            _aggregateValidator = new VacancyViewModelValidator();
        }

        [TestCase(ApprenticeshipLevel.Unknown, TrainingType.Unknown, true)]
        [TestCase(1, TrainingType.Unknown, true)]
        [TestCase(ApprenticeshipLevel.Intermediate, TrainingType.Unknown, true)]
        [TestCase(ApprenticeshipLevel.Advanced, TrainingType.Unknown, true)]
        [TestCase(ApprenticeshipLevel.Higher, TrainingType.Unknown, true)]
        [TestCase(ApprenticeshipLevel.FoundationDegree, TrainingType.Unknown, true)]
        [TestCase(ApprenticeshipLevel.Degree, TrainingType.Unknown, true)]
        [TestCase(ApprenticeshipLevel.Masters, TrainingType.Unknown, true)]
        [TestCase(8, TrainingType.Unknown, true)]
        [TestCase(ApprenticeshipLevel.Unknown, TrainingType.Frameworks, false)]
        [TestCase(1, TrainingType.Frameworks, false)]
        [TestCase(ApprenticeshipLevel.Intermediate, TrainingType.Frameworks, true)]
        [TestCase(ApprenticeshipLevel.Advanced, TrainingType.Frameworks, true)]
        [TestCase(ApprenticeshipLevel.Higher, TrainingType.Frameworks, true)]
        [TestCase(ApprenticeshipLevel.FoundationDegree, TrainingType.Frameworks, false)]
        [TestCase(ApprenticeshipLevel.Degree, TrainingType.Frameworks, true)]
        [TestCase(ApprenticeshipLevel.Masters, TrainingType.Frameworks, false)]
        [TestCase(8, TrainingType.Frameworks, false)]
        [TestCase(ApprenticeshipLevel.Unknown, TrainingType.Standards, true)]
        [TestCase(1, TrainingType.Standards, true)]
        [TestCase(ApprenticeshipLevel.Intermediate, TrainingType.Standards, true)]
        [TestCase(ApprenticeshipLevel.Advanced, TrainingType.Standards, true)]
        [TestCase(ApprenticeshipLevel.Higher, TrainingType.Standards, true)]
        [TestCase(ApprenticeshipLevel.FoundationDegree, TrainingType.Standards, true)]
        [TestCase(ApprenticeshipLevel.Degree, TrainingType.Standards, true)]
        [TestCase(ApprenticeshipLevel.Masters, TrainingType.Standards, true)]
        [TestCase(8, TrainingType.Standards, true)]
        public void ShouldRequireApprenticeshipLevel(ApprenticeshipLevel apprenticeshipLevel, TrainingType trainingType, bool expectValid)
        {
            // Arrange.
            var viewModel = new NewVacancyViewModel
            {
                TrainingType = trainingType,
                ApprenticeshipLevel = apprenticeshipLevel
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            // Act.
            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            // Assert.
            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(m => m.ApprenticeshipLevel, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.ApprenticeshipLevel, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.ApprenticeshipLevel, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.ApprenticeshipLevel, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.ApprenticeshipLevel, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(m => m.ApprenticeshipLevel, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.ApprenticeshipLevel, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.ApprenticeshipLevel, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.ApprenticeshipLevel, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.ApprenticeshipLevel, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(TrainingType.Unknown, false)]
        [TestCase(TrainingType.Frameworks, true)]
        [TestCase(TrainingType.Standards, true)]
        [TestCase(4, false)]
        public void ShouldRequireTrainingType(TrainingType trainingType, bool expectValid)
        {
            // Arrange.
            var viewModel = new NewVacancyViewModel
            {
                TrainingType = trainingType
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            // Act.
            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            // Assert.
            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(m => m.TrainingType, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.TrainingType, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.TrainingType, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.TrainingType, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.TrainingType, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(m => m.TrainingType, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.TrainingType, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.TrainingType, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.TrainingType, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.TrainingType, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, TrainingType.Unknown, true)]
        [TestCase("ABC", TrainingType.Unknown, true)]
        [TestCase(null, TrainingType.Frameworks, false)]
        [TestCase("ABC", TrainingType.Frameworks, true)]
        [TestCase(null, TrainingType.Standards, true)]
        [TestCase("ABC", TrainingType.Standards, true)]
        public void ShouldRequireFrameworkCodeName(string frameworkCodeName, TrainingType trainingType, bool expectValid)
        {
            // Arrange.
            var viewModel = new NewVacancyViewModel
            {
                TrainingType = trainingType,
                FrameworkCodeName = frameworkCodeName
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            // Act.
            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            // Assert.
            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(m => m.FrameworkCodeName, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.FrameworkCodeName, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.FrameworkCodeName, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.FrameworkCodeName, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.FrameworkCodeName, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(m => m.FrameworkCodeName, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.FrameworkCodeName, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.FrameworkCodeName, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.FrameworkCodeName, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.FrameworkCodeName, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, TrainingType.Unknown, true)]
        [TestCase("1234", TrainingType.Unknown, true)]
        [TestCase(null, TrainingType.Frameworks, true)]
        [TestCase("1234", TrainingType.Frameworks, true)]
        [TestCase(null, TrainingType.Standards, false)]
        [TestCase("1234", TrainingType.Standards, true)]
        public void ShouldRequireStandardId(string standardIdString, TrainingType trainingType, bool expectValid)
        {
            // Arrange.
            int? standardId = null;
            int parsedStandardId;
            if (int.TryParse(standardIdString, out parsedStandardId))
            {
                standardId = parsedStandardId;
            }
            var viewModel = new NewVacancyViewModel
            {
                TrainingType = trainingType,
                StandardId = standardId
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            // Act.
            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            // Assert.
            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(m => m.StandardId, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.StandardId, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.StandardId, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.StandardId, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.StandardId, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(m => m.StandardId, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.StandardId, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.StandardId, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.StandardId, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.StandardId, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase("http://www.google.com", true)]
        [TestCase("asdf", false)]
        [TestCase("asdf.asdflkjasdfl", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("http://www.google.com/", true)]
        [TestCase("http://www.google.com/someFolder", true)]
        [TestCase("http://www.google.com/someFolder/index.html", true)]
        [TestCase("http://www.google.com/someFolder/index.html?id=445667", true)]
        [TestCase("https://www.google.com/", true)]
        [TestCase("https://www.google.com/someFolder", true)]
        [TestCase("https://www.google.com/someFolder/index.html", true)]
        [TestCase("https://www.google.com/someFolder/index.html?id=445667", true)]
        [TestCase("www.google.com", true)]
        [TestCase("www.google.com/someFolder", true)]
        [TestCase("www.google.com/someFolder/index.html", true)]
        [TestCase("www.google.com/someFolder/index.html?id=445667", true)]
        [TestCase("asdf://asdf.com", false)]
        public void ShouldHaveAValidUrlIfTheVacancyIsOffline(string url, bool expectValid)
        {
            var viewModel = new NewVacancyViewModel
            {
                ApprenticeshipLevel = ApprenticeshipLevel.Advanced,
                FrameworkCodeName = "some framework code name",
                OfflineVacancy = true,
                OfflineApplicationUrl = url
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            // Act.
            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            // Assert.
            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(m => m.OfflineApplicationUrl, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(m => m.OfflineApplicationUrl, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }
    }
}
