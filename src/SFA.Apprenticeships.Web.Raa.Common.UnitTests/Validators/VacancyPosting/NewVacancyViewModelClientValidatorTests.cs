namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Validators.VacancyPosting
{
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Common.Validators.Vacancy;
    using Domain.Entities.Vacancies;
    using ViewModels.Vacancy;

    [TestFixture]
    public class NewVacancyViewModelClientValidatorTests
    {
        private NewVacancyViewModelClientValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new NewVacancyViewModelClientValidator();
        }

        [TestCase(VacancyType.Unknown, true)]
        [TestCase(VacancyType.Apprenticeship, true)]
        [TestCase(VacancyType.Traineeship, true)]
        public void VacancyTypeNotRequired(VacancyType vacancyType, bool expectValid)
        {
            var viewModel = new NewVacancyViewModel
            {
                VacancyType = vacancyType
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyType, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.VacancyType, viewModel);
            }
        }

        [TestCase(null, true)]
        [TestCase(true, true)]
        [TestCase(false, true)]
        public void OfflineVacancyNotRequired(bool? offlineVacancy, bool expectValid)
        {
            var viewModel = new NewVacancyViewModel
            {
                OfflineVacancy = offlineVacancy
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.OfflineVacancy, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.OfflineVacancy, viewModel);
            }
        }
    }
}
