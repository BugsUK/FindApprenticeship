namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.ReferenceDataProvider
{
    using System.Linq;
    using Apprenticeships.Domain.Entities.Reference;
    using Apprenticeships.Domain.Interfaces.Repositories;
    using AvService.Mappers.Version51;
    using AvService.Providers.Version51;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GetLocalAuthoritiesTests
    {
        private IReferenceDataProvider _referenceDataProvider;

        private Mock<IReferenceRepository> _referenceRepository;

        [SetUp]
        public void Setup()
        {
            _referenceRepository = new Mock<IReferenceRepository>();

            _referenceDataProvider = new ReferenceDataProvider(_referenceRepository.Object, new CountyMapper(), new RegionMapper(), new LocalAuthorityMapper());
        }

        [Test]
        public void CallsRespositoryMethod()
        {
            //Act
            _referenceDataProvider.GetLocalAuthorities();

            //Assert
            _referenceRepository.Verify(r => r.GetLocalAuthorities());
        }

        [Test]
        public void ReturnsLocalAuthorities()
        {
            var localAuthorities = new Fixture().CreateMany<LocalAuthority>(3).ToList();
            _referenceRepository.Setup(r => r.GetLocalAuthorities()).Returns(localAuthorities);

            //Act
            var localAuthoritiesList = _referenceDataProvider.GetLocalAuthorities();

            //Assert
            localAuthoritiesList.Count.Should().Be(localAuthorities.Count);
            foreach (var localAuthority in localAuthorities)
            {
                localAuthoritiesList.Any(c => c.FullName == localAuthority.FullName).Should().BeTrue();
            }
        }
    }
}