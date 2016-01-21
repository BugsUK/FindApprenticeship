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
    public class GetCountiesTests
    {
        private IReferenceDataProvider _referenceDataProvider;

        private Mock<IReferenceRepository> _referenceRepository;

        [SetUp]
        public void Setup()
        {
            _referenceRepository = new Mock<IReferenceRepository>();

            _referenceDataProvider = new ReferenceDataProvider(_referenceRepository.Object, new CountyMapper());
        }

        [Test]
        public void CallsRespositoryMethod()
        {
            //Act
            _referenceDataProvider.GetCounties();

            //Assert
            _referenceRepository.Verify(r => r.GetCounties());
        }

        [Test]
        public void ReturnsCounties()
        {
            var counties = new Fixture().CreateMany<County>(3).ToList();
            _referenceRepository.Setup(r => r.GetCounties()).Returns(counties);

            //Act
            var countiesList = _referenceDataProvider.GetCounties();

            //Assert
            countiesList.Count.Should().Be(counties.Count);
            foreach (var county in counties)
            {
                countiesList.Any(c => c.FullName == county.FullName).Should().BeTrue();
            }
        }
    }
}