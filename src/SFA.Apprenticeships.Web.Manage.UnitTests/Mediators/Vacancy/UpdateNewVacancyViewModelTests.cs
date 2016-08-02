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
    [Parallelizable]
    public class UpdateNewVacancyViewModelTests
    {
        [Test]
        public void ShouldReturnFailedValidationIfTheViewModelIsNotCorrect()
        {
            const int vacancyReferenceNumber = 1;
            var vacancy = VacancyMediatorTestHelper.GetValidVacancyViewModel(vacancyReferenceNumber);
            vacancy.NewVacancyViewModel.Title = null; // Make the vacancy invalid

            var mediator = new VacancyMediatorBuilder().Build();

            var result = mediator.UpdateVacancy(vacancy.NewVacancyViewModel);
            result.AssertValidationResult(VacancyMediatorCodes.UpdateVacancy.FailedValidation);
        }

        [Test]
        public void ShouldReturnInvalidVacancyIfTheVacancyIsNotAvailableToQA()
        {
            const int vacancyReferenceNumber = 1;
            var provider = new Mock<IVacancyQAProvider>();
            var vacancy = VacancyMediatorTestHelper.GetValidVacancyViewModel(vacancyReferenceNumber);
            var qaActionResult = new QAActionResult<NewVacancyViewModel>(QAActionResultCode.InvalidVacancy);

            provider.Setup(p => p.UpdateVacancyWithComments(vacancy.NewVacancyViewModel)).Returns(qaActionResult);

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.UpdateVacancy(vacancy.NewVacancyViewModel);
            result.AssertMessage(VacancyMediatorCodes.UpdateVacancy.InvalidVacancy, VacancyViewModelMessages.InvalidVacancy, UserMessageLevel.Error);
        }

        [Test]
        public void ShouldReturnOkIsThereIsNotAnyIssue()
        {
            const int vacancyReferenceNumber = 1;
            var provider = new Mock<IVacancyQAProvider>();
            var vacancy = VacancyMediatorTestHelper.GetValidVacancyViewModel(vacancyReferenceNumber);
            var qaActionResult = new QAActionResult<NewVacancyViewModel>(QAActionResultCode.Ok, vacancy.NewVacancyViewModel);

            provider.Setup(p => p.UpdateVacancyWithComments(vacancy.NewVacancyViewModel)).Returns(qaActionResult);

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.UpdateVacancy(vacancy.NewVacancyViewModel);
            result.AssertCode(VacancyMediatorCodes.UpdateVacancy.Ok);
            result.ViewModel.ShouldBeEquivalentTo(vacancy.NewVacancyViewModel);
        }
    }
}