namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyUploadProvider
{
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using AvService.Providers.Version51;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class UploadVacanciesTests
    {
        private Mock<IVacancyPostingService> _mockVacancyPostingService;

        private VacancyUploadProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _mockVacancyPostingService = new Mock<IVacancyPostingService>();

            _provider = new VacancyUploadProvider(_mockVacancyPostingService.Object);
        }

        [Test]
        [Ignore]
        public void ShouldUploadOneVacancy()
        {
            // Arrange.
            var request = new VacancyUploadRequest();

            // Act.
            var actualResponse = _provider.UploadVacancies(request);

            // Assert.
            var vacancy = new ApprenticeshipVacancy();

            _mockVacancyPostingService.Verify(mock =>
                mock.SaveApprenticeshipVacancy(vacancy), Times.Once);
        }
    }
}
