namespace SFA.Apprenticeships.Infrastructure.UnitTests.Security
{
    using System;
    using FluentAssertions;
    using Infrastructure.Security;
    using Infrastructure.Security.Configuration;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    [Parallelizable]
    public class AES256ProviderTests
    {
        private Mock<IConfigurationService> mockConfig = new Mock<IConfigurationService>();
        private Mock<ILogService> mockLog = new Mock<ILogService>();
        private AES256Provider providerUnderTest;
        private readonly string AesKey = "WCWB0DBPmlGvr/G9ZVPXMP3mBqpiHswC6gV/2+zeACA=";
        private readonly string AesIV = "At1KzcFvd1Fx1mKvzFegew==";

        [SetUp]
        public void Setup()
        {
            mockConfig.Setup(m => m.Get<CryptographyConfiguration>())
                .Returns(new CryptographyConfiguration() { Key = AesKey, IV = AesIV });

            providerUnderTest = new AES256Provider(mockConfig.Object, mockLog.Object);
        }

        [Test]
        public void EncryptRequiresInput()
        {
            //Arrange
            string input = null;

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => providerUnderTest.Encrypt(input));
        }

        [Test]
        public void DeryptRequiresCipherText()
        {
            //Arrange
            string cipherText = null;

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => providerUnderTest.Decrypt(cipherText));
        }

        [Test]
        public void RoundTrip()
        {
            //Arrange
            string original = "Here is some data to encrypt!";

            //Act
            var encrypted = providerUnderTest.Encrypt(original);
            string roundtrip = providerUnderTest.Decrypt(encrypted);

            //Assert
            roundtrip.ShouldBeEquivalentTo(original);

        }

        [TestCase("test", "lVs/VZWa8fpC0+J884LnGw==")]
        [TestCase("{id:1,name:'name value'}", "1DPOw7tEL+HCJ6jxO3Pz0iwTKspIDMnB9+auMdFffHI=")]
        [TestCase("here's a string", "3MBKuyWFcvN1RK+jXIJ3bg==")]
        public void EncryptString(string input, string expectedOutput)
        {
            //Arrange
            //Act
            var result = providerUnderTest.Encrypt(input);

            //Assert
            result.Should().Be(expectedOutput);
        }

        [TestCase("test", "lVs/VZWa8fpC0+J884LnGw==")]
        [TestCase("{id:1,name:'name value'}", "1DPOw7tEL+HCJ6jxO3Pz0iwTKspIDMnB9+auMdFffHI=")]
        [TestCase("here's a string", "3MBKuyWFcvN1RK+jXIJ3bg==")]
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
