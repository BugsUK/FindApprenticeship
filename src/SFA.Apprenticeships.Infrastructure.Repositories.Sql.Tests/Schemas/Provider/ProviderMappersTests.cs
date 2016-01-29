namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Provider
{
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Schemas.Provider;

    [TestFixture]
    public class ProviderMappersTests
    {
        private IMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new ProviderMappers();
        }

        [Test]
        public void ShouldCreateMap()
        {
            new ProviderMappers().Mapper.AssertConfigurationIsValid();
        }
    }
}
