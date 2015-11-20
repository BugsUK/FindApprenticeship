namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyUploadProvider
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using AvService.Mappers.Version51;
    using AvService.Providers.Version51;
    using DataContracts.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class UploadVacanciesTests
    {
        private Mock<IVacancyPostingService> _mockVacancyPostingService;
        private Mock<IVacancyUploadRequestMapper> _mockVacancyUploadRequestMapper;

        private VacancyUploadProvider _provider;

        [SetUp]
        public void SetUp()
        {
            // Mappers.
            _mockVacancyUploadRequestMapper = new Mock<IVacancyUploadRequestMapper>();

            // Vacancy Posting Service.
            _mockVacancyPostingService = new Mock<IVacancyPostingService>();

            _mockVacancyPostingService.Setup(mock =>
                mock.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(new ApprenticeshipVacancy());

            // Provider.
            _provider = new VacancyUploadProvider(
                _mockVacancyUploadRequestMapper.Object,
                _mockVacancyPostingService.Object);
        }

        [Test]
        public void ShouldReflectRequestMessageIdInResponse()
        {
            // Arrange.
            var request = new VacancyUploadRequest
            {
                MessageId = Guid.NewGuid(),
                Vacancies = new List<VacancyUploadData>()
            };

            // Act.
            var actualResponse = _provider.UploadVacancies(request);

            // Assert.
            actualResponse.MessageId.Should().Be(request.MessageId);
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldUploadOneOrMoreVacancies(int vacancyCount)
        {
            // Arrange.
            var vacancies = new List<VacancyUploadData>();

            for (var i = 0; i < vacancyCount; i++)
            {
                vacancies.Add(new VacancyUploadData());
            }

            var request = new VacancyUploadRequest
            {
                Vacancies = vacancies
            };

            // Act.
            var response = _provider.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();
            response.Vacancies.Count.Should().Be(vacancyCount);

            // Assert.
            _mockVacancyPostingService.Verify(mock =>
                mock.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Exactly(vacancyCount));
        }

        [TestCase(null)]
        [TestCase(0)]
        public void ShouldNotUploadVacanciesIfNullOrEmptyVacancyList(int? vacancyCount)
        {
            // Arrange.
            var vacancies = vacancyCount.HasValue
                ? new List<VacancyUploadData>()
                : null;

            var request = new VacancyUploadRequest
            {
                Vacancies = vacancies
            };

            // Act.
            var response = _provider.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();
            response.Vacancies.Count.Should().Be(0);

            // Assert.
            _mockVacancyPostingService.Verify(mock =>
                mock.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Never);
        }
    }
}
