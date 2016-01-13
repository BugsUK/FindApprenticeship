namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyDetailsProvider
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using SFA.Infrastructure.Interfaces;
    using Apprenticeships.Domain.Interfaces.Queries;
    using Apprenticeships.Domain.Interfaces.Repositories;
    using AvService.Mappers.Version51;
    using AvService.Providers.Version51;
    using Configuration;
    using DataContracts.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetVacancyDetailsByQueryTests
    {
        private IVacancyDetailsProvider _provider;

        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<IApprenticeshipVacancyReadRepository> _mockApprenticeshipVacancyReadRepository;

        private const string ValidEmployerInformationUrl = "http://example.com";
        private const string ValidEmployerInformationText = "Some blurb.";

        [SetUp]
        public void SetUp()
        {
            // Repository.
            _mockApprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();

            // Configuration.
            _mockConfigurationService = new Mock<IConfigurationService>();

            _mockConfigurationService.Setup(mock =>
                mock.Get<ApiConfiguration>())
                .Returns(new ApiConfiguration
                {
                    EmployerInformationUrl = ValidEmployerInformationUrl,
                    EmployerInformationText = ValidEmployerInformationText
                });

            // Mappers.
            var addressMapper = new AddressMapper();
            var vacancyDurationMapper = new VacancyDurationMapper();
            var apprenticeshipVacancyMapper = new ApprenticeshipVacancyMapper(addressMapper, vacancyDurationMapper);
            var apprenticeshipVacancyQueryMapper = new ApprenticeshipVacancyQueryMapper();

            // Provider.
            _provider = new VacancyDetailsProvider(
                _mockConfigurationService.Object,
                _mockApprenticeshipVacancyReadRepository.Object,
                apprenticeshipVacancyMapper,
                apprenticeshipVacancyQueryMapper);
        }

        [Test]
        public void ShouldReflectRequestMessageIdInResponse()
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData()
            };

            InitialiseMockRepository();

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
                VacancySearchCriteria = new VacancySearchData()
            };

            InitialiseMockRepository();

            // Act.
            var response = _provider.Get(request);

            // Assert.
            var avmsHeader = response?.SearchResults?.AVMSHeader;

            avmsHeader.Should().NotBeNull();
            avmsHeader?.ApprenticeshipVacanciesURL.Should().Be(ValidEmployerInformationUrl);
            avmsHeader?.ApprenticeshipVacanciesDescription.Should().Be(ValidEmployerInformationText);
        }

        [TestCase(0, 0)]
        [TestCase(10, 101)]
        public void ShouldReturnVacanciesFound(int searchResultsCount, int totalSearchResultsCount)
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData()
            };

            InitialiseMockRepository(searchResultsCount, totalSearchResultsCount);

            // Act.
            var response = _provider.Get(request);

            // Assert.
            var searchResults = response?.SearchResults?.SearchResults;

            searchResults.Should().NotBeNull();
            searchResults?.Count.Should().Be(searchResultsCount);
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 1, 1)]
        [TestCase(10, 11, 2)]
        [TestCase(10, 101, 11)]
        public void ShouldSetTotalPages(int searchResultsCount, int totalSearchResultsCount, int expectedTotalPages)
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData()
            };

            InitialiseMockRepository(searchResultsCount, totalSearchResultsCount);

            // Act.
            var response = _provider.Get(request);

            // Assert.
            response.Should().NotBeNull();
            response.SearchResults.TotalPages.Should().Be(expectedTotalPages);
        }

        // ReSharper disable once RedundantAssignment
        private void InitialiseMockRepository(int searchResultsCount = 5, int totalSearchResultsCount = 42)
        {
            var apprenticeshipVacancies = new List<ApprenticeshipVacancy>();

            for (var i = 0; i < searchResultsCount; i++)
            {
                apprenticeshipVacancies.Add(new ApprenticeshipVacancy());
            }

            _mockApprenticeshipVacancyReadRepository
                .Setup(mock => mock.Find(It.IsAny<ApprenticeshipVacancyQuery>(), out totalSearchResultsCount))
                .Returns(apprenticeshipVacancies);
        }
    }
}
