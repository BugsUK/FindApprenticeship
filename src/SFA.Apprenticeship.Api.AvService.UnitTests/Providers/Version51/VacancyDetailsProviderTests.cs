namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51
{
    using System;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Apprenticeships.Domain.Interfaces.Repositories;
    using AvService.Providers.Version51;
    using DataContracts.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyDetailsProviderTests
    {
        private IVacancyDetailsProvider _provider;
        private Mock<IApprenticeshipVacancyReadRepository> _mockApprenticeshipVacancyReadRepository;

        private const int ValidVacancyReferenceNumber = 5;

        [SetUp]
        public void SetUp()
        {
            _mockApprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();

            var apprenticeshipVacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = ValidVacancyReferenceNumber
            };

            _mockApprenticeshipVacancyReadRepository
                .Setup(mock => mock.Get(ValidVacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);

            _provider = new VacancyDetailsProvider(
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
                    VacancyReferenceId = ValidVacancyReferenceNumber
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
                    VacancyReferenceId = ValidVacancyReferenceNumber
                }
            };

            // Act.
            var response = _provider.Get(request);

            // Assert.
            var avmsHeader = response?.SearchResults?.AVMSHeader;

            avmsHeader.Should().NotBeNull();
            avmsHeader?.ApprenticeshipVacanciesDescription.Should().Be("TODO");
            avmsHeader?.ApprenticeshipVacanciesURL.Should().Be("TODO");
        }

        [TestCase(ValidVacancyReferenceNumber)]
        [TestCase(ValidVacancyReferenceNumber + 1)]
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
        public void ShouldReturnSingleVacancyForValidVacancyReferenceNumber()
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    VacancyReferenceId = ValidVacancyReferenceNumber
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
        public void ShouldReturnNoVacanciesForInvalidVacancyReferenceNumber()
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    VacancyReferenceId = ValidVacancyReferenceNumber + 1
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
