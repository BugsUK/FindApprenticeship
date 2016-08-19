namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyManagement
{
    using System;
    using Application.Interfaces.Service;
    using Application.Vacancy;
    using Common.Constants;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    public class DeleteVacancyTests : TestsBase
    {
        [Test]
        public void ShouldHaveAGoodResultIfDeletingAVacancyServiceWasOk()
        {
            // Arrange
            var viewModel = new DeleteVacancyViewModel();
            MockVacancyManagementProvider.Setup(x => x.Delete(It.IsAny<int>())).Returns(new ServiceResult(VacancyManagementServiceCodes.Delete.Ok));
            var sut = GetMediator();

            // Act
            var response = sut.Delete(viewModel);

            // Assert
            Assert.AreEqual(VacancyManagementMediatorCodes.DeleteVacancy.Ok, response.Code);
            Assert.AreNotEqual(UserMessageLevel.Error, response.Message.Level);
            MockVacancyManagementProvider.Verify(x => x.Delete(It.IsAny<int>()));
        }

        [Test]
        public void ShouldReturnAFailureIfDeletingAVacancyHasAnyTypeOfError()
        {
            // Arrange
            var viewModel = new DeleteVacancyViewModel();
            MockVacancyManagementProvider.Setup(x => x.Delete(It.IsAny<int>())).Returns(new ServiceResult(VacancyManagementServiceCodes.Delete.VacancyInIncorrectState));
            var sut = GetMediator();

            // Act
            var response = sut.Delete(viewModel);

            // Assert
            Assert.AreEqual(VacancyManagementMediatorCodes.DeleteVacancy.Failure, response.Code);
            Assert.AreEqual(UserMessageLevel.Error, response.Message.Level);
            MockVacancyManagementProvider.Verify(x => x.Delete(It.IsAny<int>()));
        }
    }
}