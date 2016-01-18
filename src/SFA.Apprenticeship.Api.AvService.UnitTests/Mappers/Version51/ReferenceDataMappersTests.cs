namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Reference;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using AvService.Mappers.Version51;
    using DataContracts.Version51;
    using FluentAssertions;
    using Infrastructure.Interfaces;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class ReferenceDataMappersTests
    {
        private IMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new ReferenceDataMappers();
        }

        [Test]
        public void ShouldCreateMap()
        {
            new ReferenceDataMappers().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapCounty()
        {
            //Arrange
            var source = new Fixture().Build<County>().Create();

            //Act
            var viewModel = _mapper.Map<County, CountyData>(source);

            //Assert
            viewModel.Should().NotBeNull();
            viewModel.FullName.Should().Be(source.FullName);
            viewModel.CodeName.Should().Be(source.CodeName);
        }
    }
}