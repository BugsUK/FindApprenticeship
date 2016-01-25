namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Reference;
    using AvService.Mappers.Version51;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class RegionMappersTests
    {
        private IRegionMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new RegionMapper();
        }

        [Test]
        public void ShouldMapRegion()
        {
            //Arrange
            var source = new Fixture().Build<Region>().Create();

            //Act
            var viewModel = _mapper.MapToRegionData(source);

            //Assert
            viewModel.Should().NotBeNull();
            viewModel.FullName.Should().Be(source.FullName);
            viewModel.CodeName.Should().Be(source.CodeName);
        }
    }
}