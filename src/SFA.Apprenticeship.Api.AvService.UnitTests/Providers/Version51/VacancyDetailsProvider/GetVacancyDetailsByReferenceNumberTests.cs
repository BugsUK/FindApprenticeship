namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyDetailsProvider
{
    using System;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Apprenticeships.Domain.Interfaces.Configuration;
    using Apprenticeships.Domain.Interfaces.Repositories;
    using AvService.Providers.Version51;
    using Configuration;
    using DataContracts.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetVacancyDetailsByReferenceNumberTests
    {
        private IVacancyDetailsProvider _provider;

        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<IApprenticeshipVacancyReadRepository> _mockApprenticeshipVacancyReadRepository;

        private const string ValidEmployerInformationUrl = "http://example.com";
        private const string ValidEmployerInformationText = "Some blurb.";

        private const int LiveVacancyReferenceNumber = 5;
        private const int DraftVacancyReferenceNumber = 99;

        [SetUp]
        public void SetUp()
        {
            _mockApprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();

            // Live vacancy.
            var liveApprenticeshipVacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = LiveVacancyReferenceNumber,
                Status = ProviderVacancyStatuses.Live
            };

            _mockApprenticeshipVacancyReadRepository
                .Setup(mock => mock.Get(LiveVacancyReferenceNumber))
                .Returns(liveApprenticeshipVacancy);

            // Draft vacancy.
            var draftApprenticeshipVacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = LiveVacancyReferenceNumber,
                Status = ProviderVacancyStatuses.Draft
            };

            _mockApprenticeshipVacancyReadRepository
                .Setup(mock => mock.Get(DraftVacancyReferenceNumber))
                .Returns(draftApprenticeshipVacancy);

            // Configuration.
            _mockConfigurationService = new Mock<IConfigurationService>();

            _mockConfigurationService.Setup(mock =>
                mock.Get<ApiConfiguration>())
                .Returns(new ApiConfiguration
                {
                    EmployerInformationUrl = ValidEmployerInformationUrl,
                    EmployerInformationText = ValidEmployerInformationText
                });

            // Provider.
            _provider = new VacancyDetailsProvider(
                _mockConfigurationService.Object,
                _mockApprenticeshipVacancyReadRepository.Object);
        }

        [Test]
        public void ShouldReflectRequestMessageIdInResponse()
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    VacancyReferenceId = LiveVacancyReferenceNumber
                }
            };

            // Act.
            var response = _provider.Get(request);

            // Assert.
            response.Should().NotBeNull();
            response.MessageId.ShouldBeEquivalentTo(request.MessageId);
        }

        [Test]
        public void ShouldSetSearchResultsAvmsHeader()
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                VacancySearchCriteria = new VacancySearchData
                {
                    VacancyReferenceId = LiveVacancyReferenceNumber
                }
            };

            // Act.
            var response = _provider.Get(request);

            // Assert.
            var avmsHeader = response?.SearchResults?.AVMSHeader;

            avmsHeader.Should().NotBeNull();
            avmsHeader?.ApprenticeshipVacanciesURL.Should().Be(ValidEmployerInformationUrl);
            avmsHeader?.ApprenticeshipVacanciesDescription.Should().Be(ValidEmployerInformationText);
        }

        [TestCase(LiveVacancyReferenceNumber)]
        [TestCase(LiveVacancyReferenceNumber + 1)]
        public void ShouldAlwaysSetTotalPagesToOne(int vacancyReferenceNumber)
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    VacancyReferenceId = vacancyReferenceNumber
                }
            };

            // Act.
            var response = _provider.Get(request);

            // Assert.
            response.Should().NotBeNull();
            response.SearchResults.TotalPages.Should().Be(1);
        }

        [Test]
        public void ShouldReturnSingleVacancyWhenVacancyIsLive()
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    VacancyReferenceId = LiveVacancyReferenceNumber
                }
            };

            // Act.
            var response = _provider.Get(request);

            // Assert.
            var searchResults = response?.SearchResults?.SearchResults;

            searchResults.Should().NotBeNull();
            searchResults?.Count.Should().Be(1);

            var vacancy = searchResults?.First();

            vacancy.Should().NotBeNull();
            vacancy?.VacancyReference.Should().Be(request.VacancySearchCriteria.VacancyReferenceId);
        }

        [Test]
        public void ShouldNotReturnVacancyWhenVacancyNotFound()
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    VacancyReferenceId = LiveVacancyReferenceNumber + 1
                }
            };

            // Act.
            var response = _provider.Get(request);

            // Assert.
            var searchResults = response?.SearchResults?.SearchResults;

            searchResults.Should().NotBeNull();
            searchResults?.Count.Should().Be(0);
        }

        [Test]
        public void ShouldNotReturnVacancyWhenVacancyIsNotLive()
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    VacancyReferenceId = DraftVacancyReferenceNumber
                }
            };

            // Act.
            var response = _provider.Get(request);

            // Assert.
            var searchResults = response?.SearchResults?.SearchResults;

            searchResults.Should().NotBeNull();
            searchResults?.Count.Should().Be(0);
        }
    }
}
