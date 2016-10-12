namespace SFA.Apprenticeships.Application.UnitTests.Vacancy
{
    using Apprenticeships.Application.Vacancy;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.Service;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyManagementServiceTests
    {
        [Test]
        public void ShouldFindASummaryAndUseTheDeleteStrategy()
        {
            // Arrange
            var mockDeleteVacancyStrategy = new Mock<IDeleteVacancyStrategy>();
            var mockVacancySummaryService = new Mock<IVacancySummaryService>();

            mockVacancySummaryService.Setup(x => x.GetById(It.IsAny<int>())).Returns(new VacancySummary());
            mockDeleteVacancyStrategy.Setup(x => x.Execute(It.IsAny<VacancySummary>())).Returns(new StrategyResult("code"));

            var sut = new VacancyManagementService(null, mockDeleteVacancyStrategy.Object, mockVacancySummaryService.Object);
             
            // Act
            sut.Delete(1);

            // Assert
            mockVacancySummaryService.VerifyAll();
            mockDeleteVacancyStrategy.VerifyAll();
        }

        [Test]
        public void ShouldFindASummary()
        {
            // Arrange
            var mockVacancySummaryService = new Mock<IVacancySummaryService>();

            mockVacancySummaryService.Setup(x => x.GetById(It.IsAny<int>())).Returns(new VacancySummary());

            var sut = new VacancyManagementService(null, null, mockVacancySummaryService.Object);

            // Act
            var result = sut.FindSummary(1);

            // Assert
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(VacancyManagementServiceCodes.FindSummary.Ok, result.Code);

            mockVacancySummaryService.VerifyAll();
        }

        [Test]
        public void ShouldntFindASummaryAndShouldReturnAnErrorCode()
        {
            // Arrange
            var mockVacancySummaryService = new Mock<IVacancySummaryService>();

            mockVacancySummaryService.Setup(x => x.GetById(It.IsAny<int>())).Returns(null as VacancySummary);

            var sut = new VacancyManagementService(null, null, mockVacancySummaryService.Object);

            // Act
            var result = sut.FindSummary(1);

            // Assert
            Assert.AreEqual(VacancyManagementServiceCodes.FindSummary.NotFound, result.Code);

            mockVacancySummaryService.VerifyAll();
        }
    }
}