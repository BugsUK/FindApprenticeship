namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators
{
    using Raa.Common.Validators;
    using NUnit.Framework;
    using FluentAssertions;

    [TestFixture]
    [Parallelizable]
    public class UriValidatorTests
    {

        [TestCase("https://www.google. com/spaces.html")]
        [TestCase("https://www .google.com/spaces.html")]
        [TestCase("ht tps://www.google.com/spaces.html")]
        public void DomainShouldNotContainWhiteSpace(string uriStringWithSpaces)
        {
            //Act
            bool isValid = Common.IsValidUrl(uriStringWithSpaces);

            //Assert
            isValid.Should().BeFalse("Uri should not allow spaces.");
        }

        [TestCase("https://www.google.com/spaces.html")]
        [TestCase("http://www.google.com/spaces.html")]
        public void ShouldAllowHttpOrHttpsProtocols(string uriString)
        {
            //Act
            bool isValid = Common.IsValidUrl(uriString);

            //Assert
            isValid.Should().BeTrue("Uri should allow http & https protocols.");
        }

        [TestCase("tcp://www.google.com/")]
        [TestCase("netsh://www.google.com/")]
        [TestCase("ssh://www.google.com/")]
        public void ShouldNotAllowTheseProtocols(string uriString)
        {
            //Act
            bool isValid = Common.IsValidUrl(uriString);

            //Assert
            isValid.Should().BeFalse("Uri should not allow this protocol.");
        }

        public void ShouldNotAllowEmptyUris()
        {
            //Arrange
            var uriString = string.Empty;

            //Act
            bool isValid = Common.IsValidUrl(uriString);

            //Assert
            isValid.Should().BeFalse("Uri should not be empty.");
        }

        [TestCase("www.google.com/")]
        [TestCase("m.google.com/")]
        [TestCase("www.google.mu/")]
        [TestCase("bit.ly/adsfs")]
        public void ShouldAcceptUrlWithoutProtocolPrefix(string uriString)
        {
            //Act
            bool isValid = Common.IsValidUrl(uriString);

            //Assert
            isValid.Should().BeTrue("Uri should accept the lack of a protocol prefix.");
        }

        [TestCase("www.goo")]
        [TestCase("WWW.GOO")]
        public void ShouldNotAcceptUrlWithoutDomain(string uriString)
        {
            //Act
            bool isValid = Common.IsValidUrl(uriString);

            //Assert
            isValid.Should().BeFalse("Uri should not accept the lack of a domain.");
        }
    }
}
