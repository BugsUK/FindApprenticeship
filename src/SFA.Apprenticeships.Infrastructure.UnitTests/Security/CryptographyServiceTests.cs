namespace SFA.Apprenticeships.Infrastructure.UnitTests.Security
{
    using System;
    using FluentAssertions;
    using Infrastructure.Security;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class CryptographyServiceTests
    {
        [Test]
        public void RequiresObjectToDecrypt()
        {
            //Arrange
            string objectToEncrypt = null;
            var cryptoService = new CryptographyService<string>();

            //Act && Assert
            Assert.Throws<ArgumentNullException>(()=> cryptoService.Encrypt(objectToEncrypt));
        }

        [Test]
        public void CanEncryptString()
        {
            //Arrange
            var objectToEncrypt = "test string";
            var cryptoService = new CryptographyService<string>();

            //Act
            var result = cryptoService.Encrypt(objectToEncrypt);

            //Assert
            result.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public void CanEncryptComplexObject()
        {
            //Arrange
            var objectToEncrypt = new Fixture().Create<NestedComplexObject>();
            var cryptoService = new CryptographyService<NestedComplexObject>();

            //Act
            var result = cryptoService.Encrypt(objectToEncrypt);

            //Assert
            result.Should().NotBeNullOrWhiteSpace();
        }
    }

    internal sealed class NestedComplexObject
    {
        public int Id { get; set; }

        public string PropertyName { get; set; }
    }
}
