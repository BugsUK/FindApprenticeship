namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Validators.VacancyPosting
{
    using System.Collections.Generic;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Builders;
    using Common.Validators.Vacancy;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;
    using Web.Common.UnitTests.Validators;
    using Web.Common.Validators;
    using Web.Common.ViewModels;

    [TestFixture]
    [Parallelizable]
    public class NewVacancyViewModelOfflineServerValidatorTests
    {
        private NewVacancyViewModelServerValidator _validator;
        private VacancyViewModelValidator _aggregateValidator;
        private FurtherVacancyDetailsViewModel _furtherDetailsViewModel;

        [SetUp]
        public void SetUp()
        {
            _validator = new NewVacancyViewModelServerValidator();
            _aggregateValidator = new VacancyViewModelValidator();
            _furtherDetailsViewModel = new FurtherVacancyDetailsViewModel()
            {
                Wage = new WageViewModel()
            };
        }

        [TestCase(null, false)]
        [TestCase(OfflineVacancyType.Unknown, false)]
        [TestCase(OfflineVacancyType.SingleUrl, false)]
        [TestCase(OfflineVacancyType.MultiUrl, true)]
        public void OfflineApplicationUrlNotRequired(OfflineVacancyType? offlineVacancyType, bool expectValid)
        {
            var viewModel = new NewVacancyViewModel
            {
                OfflineVacancy = true,
                OfflineVacancyType = offlineVacancyType,
                OfflineApplicationUrl = null
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_furtherDetailsViewModel).Build();

            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.OfflineApplicationUrl, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.OfflineApplicationUrl, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.ErrorsAndWarnings);
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
        [TestCase("https://www.sheffcol.ac.uk/form/Apprenticeship Application Form/?lvacancyid=VACID-352&lstudy_type=Vacancy", true)]
        public void ShouldHaveAValidUrlIfTheVacancyIsMultiLocationOffline(string url, bool expectValid)
        {
            var viewModel = new NewVacancyViewModel
            {
                OfflineVacancy = true,
                OfflineVacancyType = OfflineVacancyType.MultiUrl,
                LocationAddresses = new List<VacancyLocationAddressViewModel>
                {
                    new VacancyLocationAddressViewModel {OfflineApplicationUrl = url},
                    new VacancyLocationAddressViewModel {OfflineApplicationUrl = url},
                    new VacancyLocationAddressViewModel {OfflineApplicationUrl = url}
                },
                VacancySource = VacancySource.Raa
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_furtherDetailsViewModel).Build();

            // Act.
            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            // Assert.
            if (expectValid)
            {
                for (int i = 0; i < viewModel.LocationAddresses.Count; i++)
                {
                    var index = i;
                    _validator.ShouldNotHaveValidationErrorFor(m => m.LocationAddresses, m => m.LocationAddresses[index].OfflineApplicationUrl, index, viewModel);
                    _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.LocationAddresses, vm => vm.NewVacancyViewModel.LocationAddresses[index].OfflineApplicationUrl, index, vacancyViewModel);
                    _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.LocationAddresses, vm => vm.NewVacancyViewModel.LocationAddresses[index].OfflineApplicationUrl, index, vacancyViewModel, RuleSets.Errors);
                    _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.LocationAddresses, vm => vm.NewVacancyViewModel.LocationAddresses[index].OfflineApplicationUrl, index, vacancyViewModel, RuleSets.Warnings);
                    _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.LocationAddresses, vm => vm.NewVacancyViewModel.LocationAddresses[index].OfflineApplicationUrl, index, vacancyViewModel, RuleSets.ErrorsAndWarnings);
                }
            }
            else
            {
                for (int i = 0; i < viewModel.LocationAddresses.Count; i++)
                {
                    var index = i;
                    _validator.ShouldHaveValidationErrorFor(m => m.LocationAddresses, m => m.LocationAddresses[index].OfflineApplicationUrl, index, viewModel);
                    _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.LocationAddresses, vm => vm.NewVacancyViewModel.LocationAddresses[index].OfflineApplicationUrl, index, vacancyViewModel);
                    _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.LocationAddresses, vm => vm.NewVacancyViewModel.LocationAddresses[index].OfflineApplicationUrl, index, vacancyViewModel, RuleSets.Errors);
                    _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.LocationAddresses, vm => vm.NewVacancyViewModel.LocationAddresses[index].OfflineApplicationUrl, index, vacancyViewModel, RuleSets.Warnings);
                    _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.LocationAddresses, vm => vm.NewVacancyViewModel.LocationAddresses[index].OfflineApplicationUrl, index, vacancyViewModel, RuleSets.ErrorsAndWarnings);
                }
            }
        }
    }
}
