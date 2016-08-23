namespace SFA.Apprenticeships.Application.UnitTests.Vacancy
{
    using Apprenticeships.Application.Vacancy;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DeleteVacancyStrategyTests
    {
        [Test]
        public void ShouldDeleteADraftVacancy()
        {
            // Arrange
            var summary = new VacancySummary { Status = VacancyStatus.Draft };
            var mockVacancyWriteRepository = new Mock<IVacancyWriteRepository>();

            var sut = new DeleteVacancyStrategy(mockVacancyWriteRepository.Object);

            // Act
            var result = sut.Execute(summary);

            // Assert
            Assert.AreEqual(VacancyManagementServiceCodes.Delete.Ok, result.Code);

            mockVacancyWriteRepository.Verify(x => x.Delete(It.IsAny<int>()));
        }

        [Test]
        public void ShouldntDeleteALiveVacancy()
        {
            // Arrange
            var summary = new VacancySummary { Status = VacancyStatus.Live };
            var mockVacancyWriteRepository = new Mock<IVacancyWriteRepository>();

            var sut = new DeleteVacancyStrategy(mockVacancyWriteRepository.Object);

            // Act
            var result = sut.Execute(summary);

            // Assert
            Assert.AreEqual(VacancyManagementServiceCodes.Delete.VacancyInIncorrectState, result.Code);

            mockVacancyWriteRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }
    }
}
