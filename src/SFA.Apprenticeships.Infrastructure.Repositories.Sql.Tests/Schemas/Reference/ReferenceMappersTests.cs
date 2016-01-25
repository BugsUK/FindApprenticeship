namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Reference
{
    using Domain.Entities.Reference;
    using FluentAssertions;
    using Sql.Schemas.Reference;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;

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
            var source = new Fixture().Build<Sql.Schemas.Reference.Entities.County>().Without(c => c.PostalAddresses).Create();

            //Act
            var viewModel = _mapper.Map<Sql.Schemas.Reference.Entities.County, County>(source);

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
            var source = new Fixture().Build<Sql.Schemas.Reference.Entities.Region>().Create();

            //Act
            var viewModel = _mapper.Map<Sql.Schemas.Reference.Entities.Region, Region>(source);

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
            var source = new Fixture().Build<Sql.Schemas.Reference.Entities.LocalAuthority>().Create();

            //Act
            var viewModel = _mapper.Map<Sql.Schemas.Reference.Entities.LocalAuthority, LocalAuthority>(source);

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