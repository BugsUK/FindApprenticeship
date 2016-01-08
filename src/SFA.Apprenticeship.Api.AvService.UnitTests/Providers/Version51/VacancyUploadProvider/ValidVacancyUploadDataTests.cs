namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyUploadProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using AvService.Mappers.Version51;
    using AvService.Providers.Version51;
    using AvService.Validators;
    using Builders;
    using DataContracts.Version51;
    using Extensions;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ValidVacancyUploadDataTests
    {
        private Mock<IVacancyPostingService> _mockVacancyPostingService;
        private Mock<IVacancyUploadRequestMapper> _mockVacancyUploadRequestMapper;

        private VacancyUploadDataBuilder _vacancyUploadDataBuilder;
        private VacancyUploadProvider _provider;

        [SetUp]
        public void SetUp()
        {
            // Mappers.
            _mockVacancyUploadRequestMapper = new Mock<IVacancyUploadRequestMapper>();

            // Vacancy Posting Service.
            _mockVacancyPostingService = new Mock<IVacancyPostingService>();

            _mockVacancyPostingService.Setup(mock =>
                mock.CreateApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(new ApprenticeshipVacancy());

            // Builders.
            _vacancyUploadDataBuilder = new VacancyUploadDataBuilder();

            // Provider.
            _provider = new VacancyUploadProvider(
                new VacancyUploadDataValidator(),
                _mockVacancyUploadRequestMapper.Object,
                _mockVacancyPostingService.Object);
        }

        [Test]
        public void ShouldUploadValidVacancy()
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder.Build();

            var request = new VacancyUploadRequest
            {
                Vacancies = new List<VacancyUploadData>
                {
                    vacancyUploadData
                }
            };

            // Act.
            var response = _provider.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();
            response.Vacancies.Count.Should().Be(1);

            var vacancyUploadResultData = response.Vacancies.Single();

            vacancyUploadResultData.ShouldBeValid();
        }

        [TestCase(1)]
        [TestCase(42)]
        public void ShouldSetVacancyReferenceNumberInResponse(int vacancyReferenceNumber)
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder.Build();

            var request = new VacancyUploadRequest
            {
                Vacancies = new List<VacancyUploadData>
                {
                    vacancyUploadData
                }
            };

            _mockVacancyPostingService.Setup(mock =>
                mock.CreateApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(new ApprenticeshipVacancy
                {
                    VacancyReferenceNumber = vacancyReferenceNumber
                });

            // Act.
            var response = _provider.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();

            var vacancyUploadResultData = response.Vacancies.Single();

            vacancyUploadResultData.ReferenceNumber.Should().Be(vacancyReferenceNumber);
        }

        [Test]
        public void ShouldReflectVacancyIdInResponse()
        {
            // Arrange.
            var vacancyId = Guid.NewGuid();

            var vacancyUploadData = _vacancyUploadDataBuilder
                .WithVacancyId(vacancyId)
                .Build();

            var request = new VacancyUploadRequest
            {
                Vacancies = new List<VacancyUploadData>
                {
                    vacancyUploadData
                }
            };

            // Act.
            var response = _provider.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();

            var vacancyUploadResultData = response.Vacancies.Single();

            vacancyUploadResultData.VacancyId.Should().Be(vacancyId);
        }

        [TestCase(1)]
        [TestCase(42)]
        public void ShouldMapVacancyUploadRequestToApprenticeshipVacancy(int vacancyReferenceNumber)
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder.Build();

            var request = new VacancyUploadRequest
            {
                Vacancies = new List<VacancyUploadData>
                {
                    vacancyUploadData
                }
            };

            var vacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            _mockVacancyUploadRequestMapper.Setup(mock =>
                mock.ToApprenticeshipVacancy(vacancyUploadData))
                .Returns(vacancy);

            _mockVacancyPostingService.Setup(mock =>
                mock.CreateApprenticeshipVacancy(vacancy))
                .Returns(vacancy);

            // Act.
            var response = _provider.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();
            response.Vacancies.Count.Should().Be(1);
            response.Vacancies.Single().ReferenceNumber.Should().Be(vacancyReferenceNumber);

            _mockVacancyUploadRequestMapper.Verify(mock =>
                mock.ToApprenticeshipVacancy(vacancyUploadData), Times.Once);
        }
    }
}
