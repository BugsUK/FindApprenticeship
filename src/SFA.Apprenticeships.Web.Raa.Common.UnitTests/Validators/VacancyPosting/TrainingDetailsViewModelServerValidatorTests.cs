namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Validators.VacancyPosting
{
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Builders;
    using Common.Validators.Vacancy;
    using Domain.Entities.Raa.Vacancies;
    using ViewModels.Vacancy;
    using Web.Common.UnitTests.Validators;
    using Web.Common.Validators;

    [TestFixture]
    public class TrainingDetailsViewModelServerValidatorTests
    {
        private TrainingDetailsViewModelServerValidator _validator;
        private VacancyViewModelValidator _aggregateValidator;

        [SetUp]
        public void SetUp()
        {
            _validator = new TrainingDetailsViewModelServerValidator();
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
        [TestCase(ApprenticeshipLevel.Unknown, TrainingType.Sectors, true)]
        [TestCase(1, TrainingType.Sectors, true)]
        [TestCase(ApprenticeshipLevel.Intermediate, TrainingType.Sectors, true)]
        [TestCase(ApprenticeshipLevel.Advanced, TrainingType.Sectors, true)]
        [TestCase(ApprenticeshipLevel.Higher, TrainingType.Sectors, true)]
        [TestCase(ApprenticeshipLevel.FoundationDegree, TrainingType.Sectors, true)]
        [TestCase(ApprenticeshipLevel.Degree, TrainingType.Sectors, true)]
        [TestCase(ApprenticeshipLevel.Masters, TrainingType.Sectors, true)]
        [TestCase(8, TrainingType.Sectors, true)]
        public void ShouldRequireApprenticeshipLevel(ApprenticeshipLevel apprenticeshipLevel, TrainingType trainingType, bool expectValid)
        {
            // Arrange.
            var viewModel = new TrainingDetailsViewModel
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
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ApprenticeshipLevel, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ApprenticeshipLevel, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ApprenticeshipLevel, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ApprenticeshipLevel, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(m => m.ApprenticeshipLevel, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ApprenticeshipLevel, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ApprenticeshipLevel, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ApprenticeshipLevel, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ApprenticeshipLevel, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(TrainingType.Unknown, false)]
        [TestCase(TrainingType.Frameworks, true)]
        [TestCase(TrainingType.Standards, true)]
        [TestCase(TrainingType.Sectors, true)]
        [TestCase(4, false)]
        public void ShouldRequireTrainingType(TrainingType trainingType, bool expectValid)
        {
            // Arrange.
            var viewModel = new TrainingDetailsViewModel
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
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingType, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingType, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingType, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingType, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(m => m.TrainingType, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingType, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingType, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingType, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingType, vacancyViewModel, RuleSets.ErrorsAndWarnings);
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
            var viewModel = new TrainingDetailsViewModel
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
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.FrameworkCodeName, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.FrameworkCodeName, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.FrameworkCodeName, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.FrameworkCodeName, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(m => m.FrameworkCodeName, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.FrameworkCodeName, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.FrameworkCodeName, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.FrameworkCodeName, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.FrameworkCodeName, vacancyViewModel, RuleSets.ErrorsAndWarnings);
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
            var viewModel = new TrainingDetailsViewModel
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
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.StandardId, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.StandardId, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.StandardId, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.StandardId, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(m => m.StandardId, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.StandardId, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.StandardId, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.StandardId, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.StandardId, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, VacancyType.Unknown, true)]
        [TestCase("1234", VacancyType.Unknown, true)]
        [TestCase(null, VacancyType.Apprenticeship, true)]
        [TestCase("1234", VacancyType.Apprenticeship, true)]
        [TestCase(null, VacancyType.Traineeship, false)]
        [TestCase("1234", VacancyType.Traineeship, true)]
        public void ShouldRequireSectorCodeName(string sectorCodeName, VacancyType vacancyType, bool expectValid)
        {
            // Arrange.
            var viewModel = new TrainingDetailsViewModel
            {
                VacancyType = vacancyType,
                SectorCodeName = sectorCodeName
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
                _validator.ShouldNotHaveValidationErrorFor(m => m.SectorCodeName, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.SectorCodeName, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.SectorCodeName, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.SectorCodeName, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.SectorCodeName, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(m => m.SectorCodeName, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.SectorCodeName, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.SectorCodeName, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.SectorCodeName, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.SectorCodeName, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [Test]
        public void EmptyContactDetailsShouldBeValid()
        {
            var viewModel = new TrainingDetailsViewModel();
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _validator.ShouldNotHaveValidationErrorFor(m => m.ContactName, viewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ContactName, vacancyViewModel);
            _validator.ShouldNotHaveValidationErrorFor(m => m.ContactNumber, viewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ContactNumber, vacancyViewModel);
            _validator.ShouldNotHaveValidationErrorFor(m => m.ContactEmail, viewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ContactEmail, vacancyViewModel);
        }

        [TestCase("asdf asdf asdf asdf asdf asf asdf asdf asdf asdf asdf asdf asdf asf asdf asdf asdf asdf asdf asdf asdf asf asdf asdf asdf asdf asdf asdf asdf asf asdf asdf ", "emailemailemailemailemailemailemailemailemailemailemailemail@emailemailemailemailemailemailemaill.com", "123654987456654234")]
        [TestCase("asdf@asdf.com", "123654987456654234", "firstname lastname")]
        public void ContactDetailsShouldFailValidation(string fullName, string email, string phoneNumber)
        {
            // Arrange.
            var viewModel = new TrainingDetailsViewModel
            {
                ContactName = fullName,
                ContactEmail = email,
                ContactNumber = phoneNumber
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            // Act.
            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            // Assert.
            _validator.ShouldHaveValidationErrorFor(m => m.ContactName, viewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ContactName, vacancyViewModel);
            _validator.ShouldHaveValidationErrorFor(m => m.ContactNumber, viewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ContactNumber, vacancyViewModel);
            _validator.ShouldHaveValidationErrorFor(m => m.ContactEmail, viewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ContactEmail, vacancyViewModel);
        }

        [TestCase(null, null, null)]
        [TestCase("firstname lastname", "asdf@asdf.com", "03213465454")]
        public void ContactDetailsShouldPassValidation(string fullName, string email, string phoneNumber)
        {
            // Arrange.
            var viewModel = new TrainingDetailsViewModel
            {
                ContactName = fullName,
                ContactEmail = email,
                ContactNumber = phoneNumber
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            // Act.
            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            // Assert.
            _validator.ShouldNotHaveValidationErrorFor(m => m.ContactName, viewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ContactName, vacancyViewModel);
            _validator.ShouldNotHaveValidationErrorFor(m => m.ContactNumber, viewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ContactNumber, vacancyViewModel);
            _validator.ShouldNotHaveValidationErrorFor(m => m.ContactEmail, viewModel);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.ContactEmail, vacancyViewModel);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("<script>", false)]
        [TestCase("Desired Skill", true)]
        [TestCase(Samples.ValidFreeHtmlText, true)]
        [TestCase(Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(Samples.InvalidHtmlTextWithScript, false)]
        public void TrainingProvidedRequired(string trainingProvided, bool expectValid)
        {
            var viewModel = new TrainingDetailsViewModel
            {
                TrainingProvided = trainingProvided
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingProvided, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.TrainingProvided, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }
    }
}
