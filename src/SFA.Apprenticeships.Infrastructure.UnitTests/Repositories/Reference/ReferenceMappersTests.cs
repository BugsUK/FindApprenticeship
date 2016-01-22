namespace SFA.Apprenticeships.Infrastructure.UnitTests.Repositories.Reference
{
    using Domain.Entities.Reference;
    using FluentAssertions;
    using Infrastructure.Repositories.Reference.Mappers;
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
            var source = new Fixture().Build<NewDB.Domain.Entities.Reference.County>().Without(c => c.PostalAddresses).Create();

            //Act
            var viewModel = _mapper.Map<NewDB.Domain.Entities.Reference.County, County>(source);

            //Assert
            viewModel.Should().NotBeNull();
            viewModel.CountyId.Should().Be(source.CountyId);
            viewModel.CodeName.Should().Be(source.CodeName);
            viewModel.ShortName.Should().Be(source.ShortName);
            viewModel.FullName.Should().Be(source.FullName);
        }
    }
}