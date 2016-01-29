namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Provider
{
    using System;
    using System.Collections.Generic;
    using Common;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.Provider;
    using Sql.Schemas.Provider.Entities;
    using DomainProvider = Domain.Entities.Providers.Provider;

    [TestFixture(Category = "Integration")]
    public class ProviderRepositoryTests
    {
        private readonly IMapper _mapper = new ProviderMappers();
        private IGetOpenConnection _connection;
        private Provider _provider;
        private Guid _providerId;
        private ProviderRepository _repository;

        [SetUp]
        public void SetUpFixture()
        {
            var dbInitialiser = new DatabaseInitialiser();

            _providerId = new Guid("00000000-0000-0000-0000-000000000001");

            _provider = new Provider()
            {
                FullName = "Provider A",
                UKPrn = 1,
                DateCreated = DateTime.Now
            };

            dbInitialiser.Publish(true);

            dbInitialiser.Seed(new List<object>() { _provider });

            _connection = dbInitialiser.GetOpenConnection();
            var logger = new Mock<ILogService>();

            _repository = new ProviderRepository(_connection, _mapper, logger.Object);
        }

        [TearDown]
        public void Cleanup()
        {
            _repository.Delete(_providerId);
        }

        [Test]
        public void GetByGuid()
        {
            //Arrange

            //Act
            var result = _repository.Get(_providerId);

            //Assert
            result.Should().NotBeNull();
        }


        [Test]
        public void GetByUKPrn()
        {
            //Arrange

            //Act
            var result = _repository.Get(_provider.UKPrn.ToString());

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void Delete()
        {
            //Arrange

            //Act
            _repository.Delete(_providerId);
            var result = _repository.Get(_providerId);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public void SaveNewProvider()
        {
            //Arrange
            var providerA = new Fixture().Build<DomainProvider>()
                .With(x => x.EntityId, new Guid("00000000-0000-0000-0000-000000000000"))
                .With(x => x.Ukprn, "999").Create();
            
            //Act
            var saveResult = _repository.Save(providerA);
            var savedProvider = _repository.Get(saveResult.EntityId);

            //Assert
            Assert.AreNotEqual(saveResult.EntityId, Guid.Empty);
            Assert.AreNotEqual(savedProvider.EntityId, Guid.Empty);
            Assert.AreEqual(savedProvider.EntityId, saveResult.EntityId);
        }


        [Test]
        public void UpdateExistingProvider()
        {
            //Arrange
            var _existing = _repository.Get(_providerId);
            _existing.Name = Guid.NewGuid().ToString();

            //Act
            var saveResult = _repository.Save(_existing);
            var savedProvider = _repository.Get(saveResult.EntityId);

            //Assert
            Assert.AreNotEqual(saveResult.EntityId, Guid.Empty);
            Assert.AreNotEqual(savedProvider.EntityId, Guid.Empty);
            Assert.AreEqual(savedProvider.EntityId, saveResult.EntityId);
        }
    }
}
