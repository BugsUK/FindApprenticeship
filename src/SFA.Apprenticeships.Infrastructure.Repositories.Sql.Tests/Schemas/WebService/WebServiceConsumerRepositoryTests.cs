namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.WebService
{
    using System;
    using Common;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.WebService;

    [TestFixture(Category = "Integration")]
    public class WebServiceConsumerRepositoryTests
    {
        private readonly IMapper _mapper = new WebServiceMappers();
        private IGetOpenConnection _connection;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            var dbInitialiser = new DatabaseInitialiser();

            dbInitialiser.Publish(true);

            _connection = dbInitialiser.GetOpenConnection();
        }

        [Test]
        public void ShouldGetExistingWebServiceConsumerByExternalSystemId()
        {
            //Arrange
            var logger = new Mock<ILogService>();
            var repository = new WebServiceConsumerRepository(_connection, _mapper, logger.Object);

            //Act
            var webServiceConsumer = repository.Get(Guid.Empty);

            //Assert
            webServiceConsumer.Should().NotBeNull();
        }

        [Test]
        public void ShouldNotGetNonExistantWebServiceConsumerByExternalSystemId()
        {
            //Arrange
            var logger = new Mock<ILogService>();
            var repository = new WebServiceConsumerRepository(_connection, _mapper, logger.Object);

            //Act
            var webServiceConsumer = repository.Get(Guid.NewGuid());

            //Assert
            webServiceConsumer.Should().BeNull();
        }
    }
}