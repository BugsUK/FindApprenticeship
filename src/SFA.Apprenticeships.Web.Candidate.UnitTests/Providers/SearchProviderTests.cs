namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Locations;
    using SFA.Infrastructure.Interfaces;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Domain.Entities.Locations;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class SearchProviderTests
    {
        private Mock<ILogService> _logService;
        private Mock<ILocationSearchService> _locationSearchService;
        private ApprenticeshipCandidateWebMappers _apprenticeshipMapper;
        
        [SetUp]
        public void Setup()
        {
            _logService = new Mock<ILogService>();
            _locationSearchService = new Mock<ILocationSearchService>();
            _apprenticeshipMapper = new ApprenticeshipCandidateWebMappers();
        }

        [TestCase]
        public void ShouldReturnLocationViewModelsFromLocation()
        {
            var locations = new List<Location>
            {
                new Location {Name = "Location1", GeoPoint = new GeoPoint {Latitude = 0.1d, Longitude = 0.2d}}
            };

            _locationSearchService.Setup(x => x.FindLocation("Location1")).Returns(locations);

            var searchProvider = new SearchProvider(_locationSearchService.Object, 
                _apprenticeshipMapper, 
                _logService.Object);

            var results = searchProvider.FindLocation("Location1");
            var result = results.Locations.First();

            result.Should().NotBeNull();
            result.Latitude.Should().Be(0.1d);
            result.Longitude.Should().Be(0.2d);
        }

        [TestCase]
        public void ShouldReturnLocationViewModelsFromNullLocation()
        {
            _locationSearchService.Setup(x => x.FindLocation(It.IsAny<string>()))
                .Returns(default(IEnumerable<Location>));

            var searchProvider = new SearchProvider(_locationSearchService.Object,
                _apprenticeshipMapper,
                _logService.Object);

            var results = searchProvider.FindLocation(string.Empty);

            results.Locations.Should().BeEmpty();
        }
    }
}
