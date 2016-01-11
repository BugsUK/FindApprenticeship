namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyUploadMediator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using AvService.Mappers.Version51;
    using AvService.Validators;
    using Builders;
    using DataContracts.Version51;
    using FluentAssertions;
    using Mediators.Version51;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;
    using Extensions;
    using FluentValidation.Results;

    [TestFixture]
    public class ValidVacancyUploadDataTests
    {
        private Mock<VacancyUploadDataValidator> _mockVacancyUploadDataValidator;
        private Mock<IVacancyUploadRequestMapper> _mockVacancyUploadRequestMapper;

        private Mock<IVacancyPostingService> _mockVacancyPostingService;
        private Mock<IProviderService> _mockProviderService;

        private VacancyUploadDataBuilder _vacancyUploadDataBuilder;

        private VacancyUploadMediator _mediator;

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
            _mediator = new VacancyUploadMediator(
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
            var response = _mediator.UploadVacancies(request);

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
            var vacancyUploadData = _vacancyUploadDataBuilder
                .Build();

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
            var response = _mediator.UploadVacancies(request);

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
            var response = _mediator.UploadVacancies(request);

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

            var vacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            _mockVacancyUploadRequestMapper.Setup(mock =>
                mock.ToVacancy(vacancyUploadData, null))
                .Returns(vacancy);

            _mockVacancyPostingService.Setup(mock =>
                mock.CreateApprenticeshipVacancy(vacancy))
                .Returns(vacancy);

            // Act.
            var response = _mediator.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();
            response.Vacancies.Count.Should().Be(1);
            response.Vacancies.Single().ReferenceNumber.Should().Be(vacancyReferenceNumber);

            _mockVacancyUploadRequestMapper.Verify(mock =>
                mock.ToVacancy(vacancyUploadData, null), Times.Once);
        }
    }
}
