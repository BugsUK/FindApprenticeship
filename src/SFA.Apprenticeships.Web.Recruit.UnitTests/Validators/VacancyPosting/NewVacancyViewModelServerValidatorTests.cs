namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Common.UnitTests.Validators;
    using Common.Validators;
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
