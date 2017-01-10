using Moq;
using NUnit.Framework;
using SFA.Apprenticeships.Application.Interfaces.Locations;
using SFA.Apprenticeships.Application.Location;
#pragma warning disable 612

namespace SFA.Apprenticeships.Application.UnitTests.Services.Location
{
    [TestFixture]
    public class LocalAuthorityLookupServiceTests
    {
        [Test]
        public void GetGeoPointForShouldCallLookupProvider()
        {
            //Arrange
            const string postcode = "CV1 2WT";
            var provider = new Mock<ILocalAuthorityLookupProvider>();
            ILocalAuthorityLookupService service = new LocalAuthorityLookupService(provider.Object);

            //Act
            service.GetLocalAuthorityCode(postcode);


            //Assert
            provider.Verify(p => p.GetLocalAuthorityCode(postcode));
        }
    }
}