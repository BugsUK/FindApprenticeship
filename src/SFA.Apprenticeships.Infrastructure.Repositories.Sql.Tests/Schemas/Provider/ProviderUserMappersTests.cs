namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Provider
{
    using System;
    using System.ComponentModel;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Sql.Schemas.Provider;

    [TestFixture]
    public class ProviderUserMappersTests
    {
        private ProviderUserMappers _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new ProviderUserMappers();
        }

        [Test]
        public void ShouldCreateMap()
        {
            // Assert.
            _mapper.Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapFromDomainToSqlProviderUser()
        {
            // Arrange.
            var fixture = new Fixture();

            var source = fixture
                .Build<Domain.Entities.Users.ProviderUser>()
                .With(each => each.Ukprn, fixture.Create<int>().ToString())
                .With(each => each.PreferredSiteErn, fixture.Create<int>().ToString())
                .Create();

            // Act.
            var destination = _mapper.Map<Domain.Entities.Users.ProviderUser, Sql.Schemas.Provider.Entities.ProviderUser>(source);

            // Assert.
            destination.Should().NotBeNull();

            // TODO: destination.ProviderUserId.Should().Be(source.ProviderUserId);
            destination.ProviderId.Should().Be(source.ProviderId);
            destination.ProviderUserGuid.Should().Be(source.ProviderUserGuid);
            destination.DateCreated.Should().Be(source.DateCreated);
            destination.DateUpdated.Should().Be(source.DateUpdated);
            destination.Ukprn.Should().Be(Convert.ToInt32(source.Ukprn));
            destination.Username.Should().Be(source.Username);
            destination.Fullname.Should().Be(source.Fullname);
            destination.PreferredSiteErn.Should().Be(Convert.ToInt32(source.PreferredSiteErn));
            destination.Email.Should().Be(source.Email);
            destination.EmailVerificationCode.Should().Be(source.EmailVerificationCode);
            destination.EmailVerifiedDateTime.Should().Be(source.EmailVerifiedDate);
            destination.PhoneNumber.Should().Be(source.PhoneNumber);
        }

        [TestCase(Domain.Entities.Users.ProviderUserStatuses.Registered, 10)]
        [TestCase(Domain.Entities.Users.ProviderUserStatuses.EmailVerified, 20)]
        public void ShouldMapFromDomainToSqlProviderUserStatus(
            Domain.Entities.Users.ProviderUserStatuses providerUserStatus, int expectedProviderUserStatusId)
        {
            // Arrange.
            var fixture = new Fixture();

            var source = fixture
                .Build<Domain.Entities.Users.ProviderUser>()
                .With(each => each.Ukprn, fixture.Create<int>().ToString())
                .With(each => each.PreferredSiteErn, fixture.Create<int>().ToString())
                .With(each => each.Status, providerUserStatus)
                .Create();

            // Act.
            var destination = _mapper.Map<Domain.Entities.Users.ProviderUser, Sql.Schemas.Provider.Entities.ProviderUser>(source);

            // Assert.
            destination.Should().NotBeNull();
            destination.ProviderUserStatusId.Should().Be(expectedProviderUserStatusId);
        }

        [Test]
        public void ShouldMapFromSqlToDomainProviderUser()
        {
            // Arrange.
            var fixture = new Fixture();

            var source = fixture
                .Build<Sql.Schemas.Provider.Entities.ProviderUser>()
                .With(each => each.ProviderUserStatusId, (int)Domain.Entities.Users.ProviderUserStatuses.Registered)
                .Create();

            // Act.
            var destination = _mapper.Map<Sql.Schemas.Provider.Entities.ProviderUser, Domain.Entities.Users.ProviderUser>(source);

            // Assert.
            destination.Should().NotBeNull();

            // TODO: destination.ProviderUserId.Should().Be(source.ProviderUserId);
            destination.ProviderId.Should().Be(source.ProviderId);
            destination.ProviderUserGuid.Should().Be(source.ProviderUserGuid);
            destination.DateCreated.Should().Be(source.DateCreated);
            destination.DateUpdated.Should().Be(source.DateUpdated);
            destination.Ukprn.Should().Be(source.Ukprn.ToString());
            destination.Username.Should().Be(source.Username);
            destination.Fullname.Should().Be(source.Fullname);
            destination.PreferredSiteErn.Should().Be(source.PreferredSiteErn.ToString());
            destination.Email.Should().Be(source.Email);
            destination.EmailVerificationCode.Should().Be(source.EmailVerificationCode);
            destination.EmailVerifiedDate.Should().Be(source.EmailVerifiedDateTime);
            destination.PhoneNumber.Should().Be(source.PhoneNumber);
        }

        [Test]
        public void ShouldThrowWhenCannotMapFromSqlToDomainProviderUserStatus()
        {
            // Arrange.
            var fixture = new Fixture();

            var source = fixture
                .Build<Sql.Schemas.Provider.Entities.ProviderUser>()
                .With(each => each.ProviderUserStatusId, 42)
                .Create();

            // Act.
            Action action = () => _mapper.Map<Sql.Schemas.Provider.Entities.ProviderUser, Domain.Entities.Users.ProviderUser>(source);

            // Assert.
            action.ShouldThrow<Exception>();
        }
    }
}
