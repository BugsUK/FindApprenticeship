namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Vacancy
{
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Constants.ViewModels;
    using FluentAssertions;
    using Manage.Mediators.Vacancy;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class UpdateFurtherVacancyDetailsViewModelTests
    {
        [Test]
        public void ShouldReturnFailedValidationIfTheViewModelIsNotCorrect()
        {
            const int vacancyReferenceNumber = 1;
            var vacancy = VacancyMediatorTestHelper.GetValidVacancyViewModel(vacancyReferenceNumber);
            vacancy.FurtherVacancyDetailsViewModel.DurationComment = @"<script> </script>"; // Make the vacancy invalid

            var mediator = new VacancyMediatorBuilder().Build();

            var result = mediator.UpdateVacancy(vacancy.FurtherVacancyDetailsViewModel);
            result.AssertValidationResult(VacancyMediatorCodes.UpdateVacancy.FailedValidation);
        }

        [Test]
        public void ShouldReturnInvalidVacancyIfTheVacancyIsNotAvailableToQA()
        {
            const int vacancyReferenceNumber = 1;
            var provider = new Mock<IVacancyQAProvider>();
            var vacancy = VacancyMediatorTestHelper.GetValidVacancyViewModel(vacancyReferenceNumber);
            var qaActionResult = new QAActionResult<FurtherVacancyDetailsViewModel>(QAActionResultCode.InvalidVacancy);

            provider.Setup(p => p.UpdateVacancyWithComments(vacancy.FurtherVacancyDetailsViewModel)).Returns(qaActionResult);

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.UpdateVacancy(vacancy.FurtherVacancyDetailsViewModel);
            result.AssertMessage(VacancyMediatorCodes.UpdateVacancy.InvalidVacancy, VacancyViewModelMessages.InvalidVacancy, UserMessageLevel.Error);
        }

        [Test]
        public void ShouldReturnOkIsThereIsNotAnyIssue()
        {
            const int vacancyReferenceNumber = 1;
            var provider = new Mock<IVacancyQAProvider>();
            var vacancy = VacancyMediatorTestHelper.GetValidVacancyViewModel(vacancyReferenceNumber);
            var qaActionResult = new QAActionResult<FurtherVacancyDetailsViewModel>(QAActionResultCode.Ok, vacancy.FurtherVacancyDetailsViewModel);

            provider.Setup(p => p.UpdateVacancyWithComments(vacancy.FurtherVacancyDetailsViewModel)).Returns(qaActionResult);

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.UpdateVacancy(vacancy.FurtherVacancyDetailsViewModel);
            result.AssertCodeAndMessage(VacancyMediatorCodes.UpdateVacancy.Ok);
            result.ViewModel.ShouldBeEquivalentTo(vacancy.FurtherVacancyDetailsViewModel);
        }
    }
}