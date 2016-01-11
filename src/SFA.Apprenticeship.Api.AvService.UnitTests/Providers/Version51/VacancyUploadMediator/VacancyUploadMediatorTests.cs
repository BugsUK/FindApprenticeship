namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyUploadMediator
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using AvService.Mappers.Version51;
    using AvService.Validators;
    using DataContracts.Version51;
    using FluentAssertions;
    using FluentValidation.Results;
    using Mediators.Version51;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyUploadMediatorTests
    {
        private Mock<VacancyUploadDataValidator> _mockVacancyUploadDataValidator;
        private Mock<VacancyUploadRequestMapper> _mockVacancyUploadRequestMapper;

        private Mock<IProviderService> _mockProviderService;
        private Mock<IVacancyPostingService> _mockVacancyPostingService;

        private VacancyUploadMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            // Mappers.
            _mockVacancyUploadRequestMapper = new Mock<VacancyUploadRequestMapper>();

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

            // Provider.
            _mediator = new VacancyUploadMediator(
                _mockVacancyUploadDataValidator.Object,
                _mockVacancyUploadRequestMapper.Object,
                _mockProviderService.Object,
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
            var actualResponse = _mediator.UploadVacancies(request);

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
            var response = _mediator.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();
            response.Vacancies.Count.Should().Be(vacancyCount);

            // Assert.
            _mockVacancyPostingService.Verify(mock =>
                mock.CreateApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Exactly(vacancyCount));
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
            var response = _mediator.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();
            response.Vacancies.Count.Should().Be(0);

            // Assert.
            _mockVacancyPostingService.Verify(mock =>
                mock.CreateApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Never);
        }
    }
}
