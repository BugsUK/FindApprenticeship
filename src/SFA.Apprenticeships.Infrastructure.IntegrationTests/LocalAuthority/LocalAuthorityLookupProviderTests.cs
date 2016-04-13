namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.LocalAuthority
{
    using Application.Location;
    using Common.IoC;
    using Domain.Entities.Raa.Locations;
    using FluentAssertions;
    using Infrastructure.Postcode.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class LocalAuthorityLookupProviderTests
    {
        [SetUp]
        public void SetUp()
        {
            _container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<PostcodeRegistry>();
            });
        }

        private Container _container;

        [Test, Category("Integration")]
        public void ShouldReturnNullForAnInvalidAddressOrPostcode()
        {
            //Arrange
            const string postcode = "SW12 ZZZ";
            var provider = _container.GetInstance<ILocalAuthorityLookupProvider>();

            //Act
            var localAuthorityCode = provider.GetLocalAuthorityCode(postcode);

            //Assert
            localAuthorityCode.Should().BeNull();
        }

        [Test, Category("Integration")]
        public void ShouldReturnTheLocalAuthorityForAValidPostCode()
        {
            //Arrange
            var provider = _container.GetInstance<ILocalAuthorityLookupProvider>();
            const string postcode = "CV1 2WT";

            //Act
            var localAuthorityCode = provider.GetLocalAuthorityCode(postcode);


            //Assert
            localAuthorityCode.Should().Be("00CQ");
        }
    }
}