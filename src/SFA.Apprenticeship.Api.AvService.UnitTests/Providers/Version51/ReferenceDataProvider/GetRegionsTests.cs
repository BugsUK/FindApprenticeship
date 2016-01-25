namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.ReferenceDataProvider
{
    using System.Linq;
    using Apprenticeships.Domain.Entities.Reference;
    using Apprenticeships.Domain.Interfaces.Repositories;
    using AvService.Mappers.Version51;
    using AvService.Providers.Version51;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GetRegionsTests
    {
        private IReferenceDataProvider _referenceDataProvider;

        private Mock<IReferenceRepository> _referenceRepository;

        [SetUp]
        public void Setup()
        {
            _referenceRepository = new Mock<IReferenceRepository>();

            _referenceDataProvider = new ReferenceDataProvider(_referenceRepository.Object, new CountyMapper(), new RegionMapper(), new LocalAuthorityMapper());
        }

        [Test]
        public void CallsRespositoryMethod()
        {
            //Act
            _referenceDataProvider.GetRegions();

            //Assert
            _referenceRepository.Verify(r => r.GetRegions());
        }

        [Test]
        public void ReturnsRegions()
        {
            var regions = new Fixture().CreateMany<Region>(3).ToList();
            _referenceRepository.Setup(r => r.GetRegions()).Returns(regions);

            //Act
            var regionsList = _referenceDataProvider.GetRegions();

            //Assert
            regionsList.Count.Should().Be(regions.Count);
            foreach (var region in regions)
            {
                regionsList.Any(c => c.FullName == region.FullName).Should().BeTrue();
            }
        }
    }
}