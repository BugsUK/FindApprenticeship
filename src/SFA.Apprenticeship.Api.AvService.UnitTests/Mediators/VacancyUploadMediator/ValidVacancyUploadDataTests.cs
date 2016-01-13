namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mediators.VacancyUploadMediator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using AvService.Mappers.Version51;
    using AvService.Mediators.Version51;
    using AvService.Validators;
    using Builders;
    using DataContracts.Version51;
    using FluentAssertions;
    using FluentValidation.Results;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;
    using Providers.Version51.VacancyUploadMediator.Extensions;

    // TODO: US872: AG: rationalise VacancyUploadServiceMediator unit tests into a single file following refactor to mediator.

    [TestFixture]
    public class ValidVacancyUploadDataTests
    {
        private Mock<VacancyUploadDataValidator> _mockVacancyUploadDataValidator;
        private Mock<IVacancyUploadRequestMapper> _mockVacancyUploadRequestMapper;

        private Mock<IVacancyPostingService> _mockVacancyPostingService;
        private Mock<IProviderService> _mockProviderService;

        private VacancyUploadDataBuilder _vacancyUploadDataBuilder;

        private VacancyUploadServiceMediator _serviceMediator;

        [SetUp]
        public void SetUp()
        {
            // Mappers.
            _mockVacancyUploadRequestMapper = new Mock<IVacancyUploadRequestMapper>();

            // Services.
            _mockVacancyPostingService = new Mock<IVacancyPostingService>();
            _mockProviderService = new Mock<IProviderService>();

            _mockVacancyPostingService.Setup(mock =>
                mock.CreateApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(new ApprenticeshipVacancy());

            // Validators.
            _mockVacancyUploadDataValidator = new Mock<VacancyUploadDataValidator>();

            _mockVacancyUploadDataValidator.Setup(mock =>
                mock.Validate(It.IsAny<VacancyUploadData>()))
                .Returns(new ValidationResult());

            // Builders.
            _vacancyUploadDataBuilder = new VacancyUploadDataBuilder();

            // Provider.
            _serviceMediator = new VacancyUploadServiceMediator(
                _mockVacancyUploadDataValidator.Object,
                _mockVacancyUploadRequestMapper.Object,
                _mockProviderService.Object,
                _mockVacancyPostingService.Object);
        }

        [Test]
        public void ShouldUploadValidVacancy()
        {
            // Arrange.
            var vacancyUploadData = _vacancyUploadDataBuilder
                .Build();

            var request = new VacancyUploadRequest
            {
                Vacancies = new List<VacancyUploadData>
                {
                    vacancyUploadData
                }
            };

            // Act.
            var response = _serviceMediator.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();
            response.Vacancies.Count.Should().Be(1);

            var vacancyUploadResultData = response.Vacancies.Single();

            vacancyUploadResultData.ShouldBeValid();
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
            var response = _serviceMediator.UploadVacancies(request);

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
            var vacancyUploadData = _vacancyUploadDataBuilder
                .Build();

            var request = new VacancyUploadRequest
            {
                Vacancies = new List<VacancyUploadData>
                {
                    vacancyUploadData
                }
            };

            var vacancy = new ApprenticeshipVacancy();

            _mockVacancyUploadRequestMapper.Setup(mock =>
                mock.ToVacancy(vacancyReferenceNumber, vacancyUploadData, null))
                .Returns(vacancy);

            _mockVacancyPostingService.Setup(mock =>
                mock.GetNextVacancyReferenceNumber())
                .Returns(vacancyReferenceNumber);

            // Act.
            var response = _serviceMediator.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();
            response.Vacancies.Count.Should().Be(1);

            _mockVacancyUploadRequestMapper.Verify(mock =>
                mock.ToVacancy(vacancyReferenceNumber, vacancyUploadData, null), Times.Once);
        }
    }
}
