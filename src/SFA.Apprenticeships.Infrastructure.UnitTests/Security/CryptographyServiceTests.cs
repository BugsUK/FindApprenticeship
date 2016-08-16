namespace SFA.Apprenticeships.Infrastructure.UnitTests.Security
{
    using System;
    using FluentAssertions;
    using Infrastructure.Security;
    using Infrastructure.Security.Configuration;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    [Parallelizable]
    public class CryptographyServiceTests
    {
        private Mock<IConfigurationService> mockConfig = new Mock<IConfigurationService>();
        private Mock<ILogService> mockLog = new Mock<ILogService>();
        private AES256Provider provider;

        private readonly string AesKey = "WCWB0DBPmlGvr/G9ZVPXMP3mBqpiHswC6gV/2+zeACA=";
        private readonly string AesIV = "At1KzcFvd1Fx1mKvzFegew==";

        [SetUp]
        public void Setup()
        {
            mockConfig.Setup(m => m.Get<CryptographyConfiguration>())
                   .Returns(new CryptographyConfiguration() { Key = AesKey, IV = AesIV });

            provider = new AES256Provider(mockConfig.Object, mockLog.Object);
        }

        [Test]
        public void RequiresObjectToEncrypt()
        {
            //Arrange
            string objectToEncrypt = null;
            var cryptoService = new CryptographyService<string>(provider);

            //Act && Assert
            Assert.Throws<ArgumentNullException>(()=> cryptoService.Encrypt(objectToEncrypt));
        }

        [Test]
        public void RequiresStringToDecrypt()
        {
            //Arrange
            string stringToDecrypt = null;
            var cryptoService = new CryptographyService<string>(provider);

            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => cryptoService.Decrypt(stringToDecrypt));
        }

        [Test]
        public void RoundTrip()
        {
            //Arrange
            var original = new Fixture().Create<NestedComplexObject>();
            var cryptoService = new CryptographyService<NestedComplexObject>(provider);

            //Act
            var encrypted = cryptoService.Encrypt(original);
            var roundtrip = cryptoService.Decrypt(encrypted);

            
            //Assert
            roundtrip.ShouldBeEquivalentTo(original);
        }
    }

    internal sealed class NestedComplexObject
    {
        public int Id { get; set; }

        public string PropertyName { get; set; }
    }
}
