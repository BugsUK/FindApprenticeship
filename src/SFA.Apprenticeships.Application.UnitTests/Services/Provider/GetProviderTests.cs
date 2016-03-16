namespace SFA.Apprenticeships.Application.UnitTests.Services.Provider
{
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GetProviderTests
    {
        private const string Ukprn = "123456";

        private Provider _provider1;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _provider1 = new Fixture().Build<Provider>().Create();
        }

        [Test]
        public void FromRepository()
        {
            var repository = new Mock<IProviderReadRepository>();
            repository.Setup(r => r.GetByUkprn(Ukprn)).Returns(_provider1);
            var service = new ProviderServiceBuilder().With(repository.Object).Build();

            var provider = service.GetProvider(Ukprn);

            provider.Should().NotBeNull();
            provider.Should().Be(_provider1);
        }
    }
}