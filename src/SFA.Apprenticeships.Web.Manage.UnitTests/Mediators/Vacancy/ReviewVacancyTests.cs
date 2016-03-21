namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Vacancy
{
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Constants.ViewModels;
    using FluentAssertions;
    using FluentValidation.Results;
    using Manage.Mediators.Vacancy;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class ReviewVacancyTests
    {
        [Test]
        public void ShouldReturnTheViewModelIfAllIsOk()
        {
            const int vacancyReferenceNumber = 1;
            var viewModel = VacancyMediatorTestHelper.GetValidVacancyViewModel(vacancyReferenceNumber);

            var provider = new Mock<IVacancyQAProvider>();

            provider.Setup(p => p.ReviewVacancy(vacancyReferenceNumber)).Returns(viewModel);

            var validator = new Mock<VacancyViewModelValidator>();
            validator.Setup(v => v.Validate(It.IsAny<VacancyViewModel>()))
                .Returns(new ValidationResult());

            var mediator = new VacancyMediatorBuilder().With(validator).With(provider).Build();

            var result = mediator.ReviewVacancy(vacancyReferenceNumber);

            result.AssertCode(VacancyMediatorCodes.ReviewVacancy.Ok);
            result.ViewModel.VacancyReferenceNumber.Should().Be(vacancyReferenceNumber);
        }

        [Test]
        public void ShouldCallTheVacancyValidator()
        {
            const int vacancyReferenceNumber = 1;
            var viewModel = VacancyMediatorTestHelper.GetValidVacancyViewModel(vacancyReferenceNumber);

            var provider = new Mock<IVacancyQAProvider>();
            provider.Setup(p => p.ReviewVacancy(vacancyReferenceNumber)).Returns(viewModel);

            var validator = new Mock<VacancyViewModelValidator>();
            validator.Setup(v => v.Validate(It.IsAny<VacancyViewModel>()))
                .Returns(new ValidationResult());

            var mediator = new VacancyMediatorBuilder().With(validator).With(provider).Build();

            mediator.ReviewVacancy(vacancyReferenceNumber);

            validator.Verify(v => v.Validate(It.IsAny<VacancyViewModel>()), Times.Once);
        }

        [Test]
        public void ShouldReturnAResponseWithValidationErrorsIfThereAreValidationErrors()
        {
            const int vacancyReferenceNumber = 1;
            var viewModel = VacancyMediatorTestHelper.GetValidVacancyViewModel(vacancyReferenceNumber);
            viewModel.NewVacancyViewModel.Title = null;

            var provider = new Mock<IVacancyQAProvider>();
            provider.Setup(p => p.ReviewVacancy(vacancyReferenceNumber)).Returns(viewModel);

            var validator = new Mock<VacancyViewModelValidator>();
            var validationFailure = new ValidationFailure("NewVacancyViewModel.Title", "someError");
            validator.Setup(v => v.Validate(It.IsAny<VacancyViewModel>()))
                .Returns(new ValidationResult(new[] {validationFailure}));

            var mediator = new VacancyMediatorBuilder().With(validator).With(provider).Build();

            var result = mediator.ReviewVacancy(vacancyReferenceNumber);

            result.AssertValidationResult(VacancyMediatorCodes.ReviewVacancy.FailedValidation, true);
            result.ViewModel.VacancyReferenceNumber.Should().Be(vacancyReferenceNumber);
        }

        [Test]
        public void ShouldReturnAMessageIfTheVacancyReturnedIsNull()
        {
            const int vacancyReferenceNumber = 1;
            VacancyViewModel viewModel = null;

            var provider = new Mock<IVacancyQAProvider>();

            provider.Setup(p => p.ReviewVacancy(vacancyReferenceNumber)).Returns(viewModel);

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.ReviewVacancy(vacancyReferenceNumber);

            result.AssertMessage(VacancyMediatorCodes.ReviewVacancy.InvalidVacancy,
                VacancyViewModelMessages.InvalidVacancy, UserMessageLevel.Error);
        }
    }
}