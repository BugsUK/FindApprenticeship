namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Common.Providers;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    public class RejectVacancyTests
    {
        private const int VacancyReferenceNumber = 1;
        private readonly Mock<IVacancyPostingService> _vacancyPostingService = new Mock<IVacancyPostingService>();
        private readonly Mock<IVacancyLockingService>  _vacancyLockingService = new Mock<IVacancyLockingService>();

        [SetUp]
        public void SetUp()
        {
            var vacancy = new Vacancy
            {
                VacancyReferenceNumber = VacancyReferenceNumber
            };

            _vacancyPostingService.Setup(r => r.GetVacancyByReferenceNumber(VacancyReferenceNumber)).Returns(vacancy);
        }

        [Test]
        public void RejectVacancyShouldCallRepositorySaveWithStatusAsRejectedByQA()
        {
            //Arrange
            _vacancyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(It.IsAny<string>(), It.IsAny<Vacancy>()))
                .Returns(true);

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(_vacancyPostingService)
                    .With(_vacancyLockingService)
                    .Build();

            //Act
            var result = vacancyProvider.RejectVacancy(VacancyReferenceNumber);

            //Assert
            result.Should().Be(QAActionResult.Ok);
            _vacancyPostingService.Verify(r => r.GetVacancyByReferenceNumber(VacancyReferenceNumber));
            _vacancyPostingService.Verify(
                r =>
                    r.UpdateVacancy(
                        It.Is<Vacancy>(
                            av =>
                                av.VacancyReferenceNumber == VacancyReferenceNumber &&
                                av.Status == VacancyStatus.Referred &&
                                av.QAUserName == null)));
        }

        [Test]
        public void ShouldReturnInvalidVacancyIfTheVacancyIsNotAvailableToQA()
        {
            //Arrange
            var vacancyLockingService = new Mock<IVacancyLockingService>();

            vacancyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(It.IsAny<string>(), It.IsAny<Vacancy>()))
                .Returns(false);

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(_vacancyPostingService)
                    .With(vacancyLockingService)
                    .Build();

            var result = vacancyProvider.RejectVacancy(VacancyReferenceNumber);

            result.Should().Be(QAActionResult.InvalidVacancy);
        }
    }
}