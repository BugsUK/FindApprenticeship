namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Tests.Candidates
{
    using Application.Interfaces;
    using Domain.Entities.Candidates;
    using FluentAssertions;
    using Mongo.Candidates.Entities;
    using Mongo.Candidates.Mappers;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
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
            var source = new Fixture().Build<MongoCandidate>().Create();

            //Act
            var candidateSummary = _mapper.Map<MongoCandidate, CandidateSummary>(source);

            //Assert
            candidateSummary.Should().NotBeNull();
            candidateSummary.EntityId.Should().Be(source.EntityId);
            candidateSummary.FirstName.Should().Be(source.RegistrationDetails.FirstName);
            candidateSummary.LastName.Should().Be(source.RegistrationDetails.LastName);
            candidateSummary.Address.Should().NotBeNull();
            candidateSummary.Address.Postcode.Should().Be(source.RegistrationDetails.Address.Postcode);
            candidateSummary.DateOfBirth.Should().Be(source.RegistrationDetails.DateOfBirth);
        }
    }
}