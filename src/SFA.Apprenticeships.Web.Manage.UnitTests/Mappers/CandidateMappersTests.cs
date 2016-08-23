namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mappers
{
    using Domain.Entities.Candidates;
    using FluentAssertions;
    using Manage.Mappers;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using ViewModels;

    [TestFixture]
    [Parallelizable]
    public class CandidateMappersTests
    {
        private IMapper _mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            _mapper = new CandidateMappers();
        }

        [Test]
        public void ShouldCreateMap()
        {
            new CandidateMappers().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapCandidateSummaryViewModel()
        {
            //Arrange
            var source = new Fixture().Build<CandidateSummary>().Create();

            //Act
            var viewModel = _mapper.Map<CandidateSummary, CandidateSummaryViewModel>(source);

            //Assert
            viewModel.Should().NotBeNull();
            viewModel.Id.Should().Be(source.EntityId);
            viewModel.Name.Should().Be(source.FirstName + " " + source.LastName);
            viewModel.Address.Should().NotBeNull();
            viewModel.Address.Postcode.Should().Be(source.Address.Postcode.ToUpper());
            viewModel.DateOfBirth.Should().Be(source.DateOfBirth);
        }
    }
}