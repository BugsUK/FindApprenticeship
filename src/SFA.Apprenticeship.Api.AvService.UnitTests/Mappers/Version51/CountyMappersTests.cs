namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Reference;
    using AvService.Mappers.Version51;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class CountyMappersTests
    {
        private ICountyMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new CountyMapper();
        }

        [Test]
        public void ShouldMapCounty()
        {
            //Arrange
            var source = new Fixture().Build<County>().Create();

            //Act
            var viewModel = _mapper.MapToCountyData(source);

            //Assert
            viewModel.Should().NotBeNull();
            viewModel.FullName.Should().Be(source.FullName);
            viewModel.CodeName.Should().Be(source.CodeName);
        }
    }
}