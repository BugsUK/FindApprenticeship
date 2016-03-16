namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Reference
{
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using Schemas.Reference;
    using Schemas.Reference.Entities;

    [TestFixture]
    public class ReferenceMappersTests
    {
        private IMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new ReferenceMappers();
        }

        [Test]
        public void ShouldCreateMap()
        {
            new ReferenceMappers().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapCounty()
        {
            //Arrange
            var source = new Fixture().Build<County>().Without(c => c.PostalAddresses).Create();

            //Act
            var viewModel = _mapper.Map<County, County>(source);

            //Assert
            viewModel.Should().NotBeNull();
            viewModel.CountyId.Should().Be(source.CountyId);
            viewModel.CodeName.Should().Be(source.CodeName);
            viewModel.ShortName.Should().Be(source.ShortName);
            viewModel.FullName.Should().Be(source.FullName);
        }

        [Test]
        public void ShouldMapRegion()
        {
            //Arrange
            var source = new Fixture().Build<Region>().Create();

            //Act
            var viewModel = _mapper.Map<Region, Region>(source);

            //Assert
            viewModel.Should().NotBeNull();
            viewModel.RegionId.Should().Be(source.RegionId);
            viewModel.CodeName.Should().Be(source.CodeName);
            viewModel.ShortName.Should().Be(source.ShortName);
            viewModel.FullName.Should().Be(source.FullName);
        }

        [Test]
        public void ShouldMapLocalAuthority()
        {
            //Arrange
            var source = new Fixture().Build<LocalAuthority>().Create();

            //Act
            var viewModel = _mapper.Map<LocalAuthority, LocalAuthority>(source);

            //Assert
            viewModel.Should().NotBeNull();
            viewModel.LocalAuthorityId.Should().Be(source.LocalAuthorityId);
            viewModel.CodeName.Should().Be(source.CodeName);
            viewModel.ShortName.Should().Be(source.ShortName);
            viewModel.FullName.Should().Be(source.FullName);
            viewModel.County.Should().NotBeNull();
            viewModel.County.CountyId.Should().Be(source.County.CountyId);
            viewModel.County.CodeName.Should().Be(source.County.CodeName);
            viewModel.County.ShortName.Should().Be(source.County.ShortName);
            viewModel.County.FullName.Should().Be(source.County.FullName);
        }
    }
}