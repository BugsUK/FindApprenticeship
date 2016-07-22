namespace SFA.Apprenticeships.Infrastructure.UnitTests.Security
{
    using System;
    using FluentAssertions;
    using SFA.Apprenticeships.Infrastructure.Security;
    using SFA.Apprenticeships.Infrastructure.Security.Configuration;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class AES256ProviderTests
    {
        private Mock<IConfigurationService> mockConfig = new Mock<IConfigurationService>();
        private AES256Provider providerUnderTest;
        private readonly string AesPassword = "test";

        [SetUp]
        public void Setup()
        {
            mockConfig.Setup(m => m.Get<CryptographyConfiguration>())
                .Returns(new CryptographyConfiguration() { Password = AesPassword });

            providerUnderTest = new AES256Provider(mockConfig.Object);
        }

        [Test]
        public void RequiresInput()
        {
            //Arrange
            string input = null;

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => providerUnderTest.Encrypt(input));
        }

        [TestCase("test", @"U2FsdGVkX1+/GYd73GeWULMITutNz+CaWZs3Q+2Kd+I=")]
        [TestCase("{id:1,name:'name value'}", @"U2FsdGVkX18V/cOOrVz06jyXCtSYsSOQ5H4pLbooj8HI5SHRczxUIV37drBe90AU")]
        [TestCase("here's a string", @"U2FsdGVkX1+BcbPXAlzh2fw1BBNE8zfkctLdLh14Roo=")]
        public void EncryptString(string input, string expectedOutput)
        {
            //Arrange
            //Act
            var result = providerUnderTest.Encrypt(input);

            //Assert
            result.Should().Be(expectedOutput);
        }

        [TestCase("test", @"U2FsdGVkX1+/GYd73GeWULMITutNz+CaWZs3Q+2Kd+I=")]
        [TestCase("{id:1,name:'name value'}", @"U2FsdGVkX18V/cOOrVz06jyXCtSYsSOQ5H4pLbooj8HI5SHRczxUIV37drBe90AU")]
        [TestCase("here's a string", @"U2FsdGVkX1+BcbPXAlzh2fw1BBNE8zfkctLdLh14Roo=")]
        public void DecryptString(string expectedOutput, string input)
        {
            //Arrange
            //Act
            var result = providerUnderTest.Decrypt(input);

            //Assert
            result.Should().Be(expectedOutput);
        }

    }
}
