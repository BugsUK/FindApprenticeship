namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.ReferenceDataProvider
{
    using Apprenticeships.Domain.Interfaces.Repositories;
    using AvService.Providers.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetCountiesTests
    {
        private IReferenceDataProvider _referenceDataProvider;

        private Mock<IReferenceRepository> _referenceRepository;

        [SetUp]
        public void Setup()
        {
            _referenceRepository = new Mock<IReferenceRepository>();

            _referenceDataProvider = new ReferenceDataProvider();
        }

        [Test]
        public void CallsRespositoryMethod()
        {
            //Act
            _referenceDataProvider.GetCounties();

            //Assert

        }
    }
}