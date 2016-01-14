namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Locations;
    using AvService.Mappers.Version51;
    using FluentAssertions;
    using NUnit.Framework;

    // REF: AV: NavmsEDSFacade.cs
    // REF: AV: NavmsPIMSFacade.cs

    // TODO: API: ShouldAlwaysMapAddressLine5ToNull()

    [TestFixture]
    public class AddressMapperTests
    {
        private IAddressMapper _addressMapper;

        [SetUp]
        public void SetUp()
        {
            _addressMapper = new AddressMapper();
        }

        [Test]
        public void ShouldReturnNullAddressIfOriginalAddressIsNull()
        {
            // Act.
            var mappedAddress = _addressMapper.MapToAddressData(null);

            // Assert.
            mappedAddress.Should().BeNull();
        }

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
            var mappedAddress = _addressMapper.MapToAddressData(address);

            // Assert.
            mappedAddress.Should().NotBeNull();
            mappedAddress.AddressLine1.Should().Be(expectedAddressLine1);
        }

        [TestCase("A", "A")]
        [TestCase("", "")]
        [TestCase(null, "")]
        public void ShouldMapPostcode(string originalPostcode, string expectedPostcode)
        {
            // Arrange.
            var address = new Address
            {
                Postcode = originalPostcode
            };

            // Act.
            var mappedAddress = _addressMapper.MapToAddressData(address);

            // Assert.
            mappedAddress.Should().NotBeNull();
            mappedAddress.PostCode.Should().Be(expectedPostcode);
        }
    }
}
