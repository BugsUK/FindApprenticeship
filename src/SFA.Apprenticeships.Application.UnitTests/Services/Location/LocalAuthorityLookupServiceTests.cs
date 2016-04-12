using Moq;
using NUnit.Framework;
using SFA.Apprenticeships.Application.Interfaces.Locations;
using SFA.Apprenticeships.Application.Location;

namespace SFA.Apprenticeships.Application.UnitTests.Services.Location
{
    [TestFixture]
    public class LocalAuthorityLookupServiceTests
    {
        [Test]
        public void GetGeoPointForShouldCallLookupProvider()
        {
            const string postcode = "CV1 2WT";
            var provider = new Mock<ILocalAuthorityLookupProvider>();
            ILocalAuthorityLookupService service = new LocalAuthorityLookupService(provider.Object);
            service.GetLocalAuthorityCode(postcode);

            provider.Verify(p => p.GetLocalAuthorityCode(postcode));
        }
    }
}