namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Recruit.Validators.VacancyPosting;
    using ViewModels.Vacancy;

    [TestFixture]
    public class NewVacancyViewModelServerValidatorTests
    {
        private NewVacancyViewModelServerValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new NewVacancyViewModelServerValidator();
        }

        [TestCase(ApprenticeshipLevel.Unknown, false)]
        [TestCase(ApprenticeshipLevel.Intermediate, true)]
        [TestCase(ApprenticeshipLevel.Higher, true)]
        [TestCase(ApprenticeshipLevel.Advanced, true)]
        [TestCase(4, false)]
        public void ShouldRequireApprenticeshipLevel(ApprenticeshipLevel apprenticeshipLevel, bool expectValid)
        {
            // Arrange.
            var viewModel = new NewVacancyViewModel
            {
                ApprenticeshipLevel = apprenticeshipLevel
            };

            // Act.
            _validator.Validate(viewModel);

            // Assert.
            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(m => m.ApprenticeshipLevel, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(m => m.ApprenticeshipLevel, viewModel);
            }
        }

        [TestCase(null, false)]
        [TestCase("ABC", true)]
        public void ShouldRequireFrameworkCodeName(string frameworkCodeName, bool expectValid)
        {
            // Arrange.
            var viewModel = new NewVacancyViewModel
            {
                FrameworkCodeName = frameworkCodeName
            };

            // Act.
            _validator.Validate(viewModel);

            // Assert.
            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(m => m.FrameworkCodeName, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(m => m.FrameworkCodeName, viewModel);
            }
        }
    }
}
