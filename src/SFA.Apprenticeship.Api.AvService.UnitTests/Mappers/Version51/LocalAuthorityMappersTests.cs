namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Reference;
    using AvService.Mappers.Version51;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class LocalAuthorityMappersTests
    {
        private ILocalAuthorityMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new LocalAuthorityMapper();
        }

        [Test]
        public void ShouldMapLocalAuthority()
        {
            //Arrange
            var source = new Fixture().Build<LocalAuthority>().Create();

            //Act
            var viewModel = _mapper.MapToLocalAuthorityData(source);

            //Assert
            viewModel.Should().NotBeNull();
            viewModel.FullName.Should().Be(source.FullName);
            viewModel.ShortName.Should().Be(source.ShortName);
        }
    }
}