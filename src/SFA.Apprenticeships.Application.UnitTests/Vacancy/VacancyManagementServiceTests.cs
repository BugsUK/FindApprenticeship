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
            var mockVacancyReadRepository = new Mock<IVacancyReadRepository>();

            mockVacancyReadRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new VacancySummary());
            mockDeleteVacancyStrategy.Setup(x => x.Execute(It.IsAny<VacancySummary>())).Returns(new StrategyResult("code"));

            var sut = new VacancyManagementService(mockVacancyReadRepository.Object, mockDeleteVacancyStrategy.Object);
             
            // Act
            sut.Delete(1);

            // Assert
            mockVacancyReadRepository.VerifyAll();
            mockDeleteVacancyStrategy.VerifyAll();
        }

        [Test]
        public void ShouldFindASummary()
        {
            // Arrange
            var mockVacancyReadRepository = new Mock<IVacancyReadRepository>();

            mockVacancyReadRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new VacancySummary());

            var sut = new VacancyManagementService(mockVacancyReadRepository.Object, null);

            // Act
            var result = sut.FindSummary(1);

            // Assert
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(VacancyManagementServiceCodes.FindSummary.Ok, result.Code);

            mockVacancyReadRepository.VerifyAll();
        }

        [Test]
        public void ShouldntFindASummaryAndShouldReturnAnErrorCode()
        {
            // Arrange
            var mockVacancyReadRepository = new Mock<IVacancyReadRepository>();

            mockVacancyReadRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(null as VacancySummary);

            var sut = new VacancyManagementService(mockVacancyReadRepository.Object, null);

            // Act
            var result = sut.FindSummary(1);

            // Assert
            Assert.AreEqual(VacancyManagementServiceCodes.FindSummary.NotFound, result.Code);

            mockVacancyReadRepository.VerifyAll();
        }
    }
}