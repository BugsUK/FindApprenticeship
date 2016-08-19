namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyManagement
{
    using Application.Interfaces.Service;
    using Application.Vacancy;
    using Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    public class ConfirmDeleteActionTests : TestsBase
    {
        [Test]
        public void ShouldFindASummaryAndReturnOk()
        {
            // Arrange
            var viewModel = new DeleteVacancyViewModel();
            MockVacancyManagementProvider.Setup(x => x.FindSummary(It.IsAny<int>())).Returns(new ServiceResult<VacancySummary>(VacancyManagementServiceCodes.FindSummary.Ok, new VacancySummary() { Title = "title"}));
            var sut = GetMediator();

            // Act
            var response = sut.ConfirmDelete(viewModel);

            // Assert
            Assert.AreEqual(VacancyManagementMediatorCodes.ConfirmDelete.Ok, response.Code);
            Assert.That(response.ViewModel.VacancyTitle, Is.Not.Null.And.Not.Empty);
            MockVacancyManagementProvider.Verify(x => x.FindSummary(It.IsAny<int>()));
        }

        [Test]
        public void ShouldReturnAErrorIfTheVacancyIsMissing()
        {
            // Arrange
            var viewModel = new DeleteVacancyViewModel();
            MockVacancyManagementProvider.Setup(x => x.FindSummary(It.IsAny<int>())).Returns(new ServiceResult<VacancySummary>(VacancyManagementServiceCodes.FindSummary.NotFound, null));
            var sut = GetMediator();

            // Act
            var response = sut.ConfirmDelete(viewModel);

            // Assert
            Assert.AreEqual(VacancyManagementMediatorCodes.ConfirmDelete.NotFound, response.Code);
            MockVacancyManagementProvider.Verify(x => x.FindSummary(It.IsAny<int>()));
        }
    }
}