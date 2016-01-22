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
    }
}