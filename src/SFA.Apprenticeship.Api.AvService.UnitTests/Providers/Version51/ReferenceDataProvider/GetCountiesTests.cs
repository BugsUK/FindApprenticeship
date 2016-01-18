namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.ReferenceDataProvider
{
    using AvService.Providers.Version51;
    using NUnit.Framework;

    [TestFixture]
    public class GetCountiesTests
    {
        private IReferenceDataProvider _referenceDataProvider;

        private IReferenceDataRepository _referenceDataRepository;

        [Test]
        public void CallsRespositoryMethod()
        {
            //Act
            _referenceDataProvider.GetCounties();

            //Assert

        }
    }
}