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

        private ProviderRepository _repository;

        [SetUp]
        public void SetUpFixture()
        {
            var dbInitialiser = new DatabaseInitialiser();

            _provider = new Provider()
            {
                ProviderId = 1,
                // Upin = 456,
                FullName = "Acme Corp",
                Ukprn = 678,
                // TradingName = "A Trading Name Company",
                // IsContracted = true,
                // ContractedFrom = DateTime.Today.AddDays(-100),
                // ContractedTo = DateTime.Today.AddDays(100),
                // ProviderStatusTypeId = (int)ProviderStatuses.Activated,
                // IsNasProvider = false,
                // OriginalUpin = 901
            };

            dbInitialiser.Publish(false);

            dbInitialiser.Seed(new List<object>()
            {
                _provider            
            });

            _connection = dbInitialiser.GetOpenConnection();

            var logger = new Mock<ILogService>();

            _repository = new ProviderRepository(_connection, _mapper, logger.Object);
        }

        [Test]
        [Ignore]
        public void GetByProviderId()
        {
            // Act.
            var provider = _repository.Get(_provider.ProviderId);

            // Assert.
            provider.Should().NotBeNull();
        }


        [Test]
        [Ignore]
        public void GetByUkprn()
        {
            // Act.
            var provider = _repository.Get(_provider.Ukprn.ToString());

            // Assert.
            provider.Should().NotBeNull();
        }

        [Test]
        [Ignore]
        public void Delete()
        {
            // Act.
            _repository.Delete(_provider.ProviderId);

            var provider = _repository.Get(_provider.ProviderId);

            // Assert.
            provider.Should().BeNull();
        }

        [Test]
        [Ignore]
        public void SaveNewProvider()
        {
            // Arrange.
            var provider = new Fixture().Build<DomainProvider>()
                .With(x => x.ProviderId, 1)
                .With(x => x.Ukprn, "999")
                .Create();

            // Act.
            var savedProvider = _repository.Save(provider);
            var gotProvider = _repository.Get(savedProvider.ProviderId);

            // Assert.
            Assert.AreNotEqual(savedProvider.ProviderId, 0);

            Assert.AreNotEqual(gotProvider.ProviderId, 0);
            Assert.AreEqual(gotProvider.ProviderId, savedProvider.ProviderId);
            Assert.AreEqual(gotProvider.Name, provider.Name);
            Assert.AreEqual(gotProvider.Ukprn, provider.Ukprn);
        }


        [Test]
        [Ignore]
        public void UpdateExistingProvider()
        {
            // Arrange.
            var existingProvider = _repository.Get(_provider.ProviderId);

            existingProvider.Name = Guid.NewGuid().ToString();

            // Act.
            var savedProvider = _repository.Save(existingProvider);
            var gotProvider = _repository.Get(savedProvider.ProviderId);

            // Assert.
            Assert.AreNotEqual(savedProvider.ProviderId, Guid.Empty);

            Assert.AreNotEqual(gotProvider.ProviderId, Guid.Empty);
            Assert.AreEqual(gotProvider.ProviderId, savedProvider.ProviderId);
        }
    }
}
