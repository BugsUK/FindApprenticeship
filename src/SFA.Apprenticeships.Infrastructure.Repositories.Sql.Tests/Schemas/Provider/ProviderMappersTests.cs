namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Provider
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Sql.Schemas.Provider;
    using Database = Sql.Schemas.Provider.Entities;
    using Domain = Domain.Entities.Providers;

    [TestFixture]
    public class ProviderMappersTests
    {
        private ProviderMappers _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new ProviderMappers();
        }

        [Test]
        public void ShouldCreateMap()
        {
            // Assert.
            _mapper.Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapFromDatabaseToDomainProvider()
        {
            // Arrange.
            var fixture = new Fixture();

            var source = fixture
                .Build<Database.Provider>()
                .Create();

            // Act.
            var destination = _mapper.Map<Database.Provider, Domain.Provider>(source);

            // Assert.
            destination.Should().NotBeNull();
            destination.ProviderId.Should().Be(source.ProviderId);
            destination.Ukprn.Should().Be(Convert.ToString(source.Ukprn));
            destination.Name.Should().Be(source.FullName);
        }

        [Test]
        public void ShouldMapFromDomainToDatabaseProvider()
        {
            // Arrange.
            var fixture = new Fixture();

            var source = fixture
                .Build<Domain.Provider>()
                .With(each => each.Ukprn, Convert.ToString(fixture.Create<int>()))
                .Create();

            // Act.
            var destination = _mapper.Map<Domain.Provider, Database.Provider>(source);

            // Assert.
            destination.Should().NotBeNull();
            destination.ProviderId.Should().Be(source.ProviderId);
            destination.Ukprn.Should().Be(Convert.ToInt32(source.Ukprn));
            destination.FullName.Should().Be(source.Name);
        }
    }
}
