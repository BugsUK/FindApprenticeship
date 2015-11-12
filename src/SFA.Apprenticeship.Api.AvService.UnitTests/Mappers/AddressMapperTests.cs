namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mappers
{
    using Apprenticeships.Domain.Entities.Locations;
    using AvService.Mappers.Version51;
    using FluentAssertions;
    using NUnit.Framework;

    // REF: AV: NavmsEDSFacade.cs
    // REF: AV: NavmsPIMSFacade.cs

    // TODO: ShouldAlwaysMapAddressLine5ToNull()

    [TestFixture]
    public class AddressMapperTests
    {
        [TestCase("A", "A")]
        [TestCase("", "")]
        [TestCase(null, "")]
        public void ShouldMapAddressLine1(string originalAddressLine1, string expectedAddressLine1)
        {
            // Arrange.
            var address = new Address
            {
                AddressLine1 = originalAddressLine1
            };

            // Act.
            var mappedAddress = AddressMapper.MapToAddressData(address);

            // Assert.
            mappedAddress.Should().NotBeNull();
            mappedAddress.AddressLine1.Should().Be(expectedAddressLine1);
        }
    }
}
