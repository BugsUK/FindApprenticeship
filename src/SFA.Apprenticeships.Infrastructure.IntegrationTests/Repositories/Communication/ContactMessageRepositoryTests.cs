namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Repositories.Communication
{
    using System;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Repositories;
    using Infrastructure.Repositories.Mongo.Communication.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NUnit.Framework;

    [TestFixture]
    public class ContactMessageRepositoryTests : RepositoryIntegrationTest
    {
        private const string CollectionName = "contactmessages";
        private static readonly Guid TestUserId = Guid.NewGuid();

        private IContactMessageRepository _repository;

        private MongoDatabase _database;
        private MongoCollection<MongoContactMessage> _collection;
        private IMongoQuery _query;

        [SetUp]
        public void SetUp()
        {
            _repository = Container.GetInstance<IContactMessageRepository>();

            var mongoConnectionString = MongoConfiguration.CommunicationsDb;
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            _database = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName);

            _collection = _database.GetCollection<MongoContactMessage>(CollectionName);
            _query = Query.EQ("UserId", TestUserId);
            _collection.Remove(_query);
        }

        [TearDown]
        public void TearDown()
        {
            _collection.Remove(_query);
        }

        [Test, Category("Integration")]
        public void ShouldSaveContactMessage()
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                UserId = TestUserId,
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Enquiry = "I've forgotten my password",
                Details = "I've still forgotten my password"
            };

            // Act.
            _repository.Save(contactMessage);

            // Assert.
            var mongoContactMessage = _collection.FindOne(_query);

            mongoContactMessage.UserId.Should().Be(contactMessage.UserId);
            mongoContactMessage.Name.Should().Be(contactMessage.Name);
            mongoContactMessage.Email.Should().Be(contactMessage.Email);
            mongoContactMessage.Enquiry.Should().Be(contactMessage.Enquiry);
            mongoContactMessage.Details.Should().Be(contactMessage.Details);
        }

        [Test, Category("Integration")]
        public void ShouldNotSupportDeleteContactMessage()
        {
            // Act.
            Action action = () => _repository.Delete(TestUserId);

            // Assert.
            action.ShouldThrow<NotSupportedException>();
        }
    }
}
