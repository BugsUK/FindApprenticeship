namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mediators.Version51.VacancyUploadMediator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using AvService.Mappers.Version51;
    using AvService.Mediators.Version51;
    using AvService.Providers;
    using AvService.Validators.Version51;
    using Builders.Version51;
    using DataContracts.Version51;
    using FluentAssertions;
    using FluentValidation.Results;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;
    using Extensions;

    // TODO: US872: AG: rationalise VacancyUploadServiceMediator unit tests into a single file following refactor to mediator.

    [TestFixture]
    public class VacancyUploadMediatorTests
    {
        private Mock<VacancyUploadDataValidator> _mockVacancyUploadDataValidator;
        private Mock<IVacancyUploadRequestMapper> _mockVacancyUploadRequestMapper;

        private Mock<IWebServiceAuthenticationProvider> _mockWebServiceAuthenticationProvider;
        private Mock<IProviderService> _mockProviderService;
        private Mock<IVacancyPostingService> _mockVacancyPostingService;

        private VacancyUploadDataBuilder _vacancyUploadDataBuilder;

        private VacancyUploadServiceMediator _vacancyUploadServiceMediator;

        [SetUp]
        public void SetUp()
        {
            // Mappers.
            _mockVacancyUploadRequestMapper = new Mock<IVacancyUploadRequestMapper>();

            // Providers.
            _mockWebServiceAuthenticationProvider = new Mock<IWebServiceAuthenticationProvider>();

            _mockWebServiceAuthenticationProvider.Setup(mock => mock
                .Authenticate(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(WebServiceAuthenticationResult.Authenticated);

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
            _vacancyUploadServiceMediator = new VacancyUploadServiceMediator(
                _mockVacancyUploadDataValidator.Object,
                _mockVacancyUploadRequestMapper.Object,
                _mockWebServiceAuthenticationProvider.Object,
                _mockProviderService.Object,
                _mockVacancyPostingService.Object);
        }

        [Test]
        public void ShouldThrowIfRequestIsNull()
        {
            // Act.
            Action action = () => _vacancyUploadServiceMediator.UploadVacancies(default(VacancyUploadRequest));

            // Assert.
            action.ShouldThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ShouldAuthenticateRequest()
        {
            // Arrange.
            var request = new VacancyUploadRequest
            {
                ExternalSystemId = Guid.NewGuid(),
                PublicKey = "dontletmein"
            };

            _mockWebServiceAuthenticationProvider.Reset();

            _mockWebServiceAuthenticationProvider.Setup(mock => mock
                .Authenticate(request.ExternalSystemId, request.PublicKey))
                .Returns(WebServiceAuthenticationResult.AuthenticationFailed);

            // Act.
            Action action = () => _vacancyUploadServiceMediator.UploadVacancies(request);

            // Assert.
            action.ShouldThrowExactly<SecurityException>();
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
            var actualResponse = _vacancyUploadServiceMediator.UploadVacancies(request);

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
            var response = _vacancyUploadServiceMediator.UploadVacancies(request);

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
            var response = _vacancyUploadServiceMediator.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();
            response.Vacancies.Count.Should().Be(0);

            // Assert.
            _mockVacancyPostingService.Verify(mock =>
                mock.CreateApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Never);
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
            var response = _vacancyUploadServiceMediator.UploadVacancies(request);

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
            var response = _vacancyUploadServiceMediator.UploadVacancies(request);

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
            var response = _vacancyUploadServiceMediator.UploadVacancies(request);

            // Assert.
            response.Should().NotBeNull();
            response.Vacancies.Count.Should().Be(1);

            _mockVacancyUploadRequestMapper.Verify(mock =>
                mock.ToVacancy(vacancyReferenceNumber, vacancyUploadData, null), Times.Once);
        }
    }
}
