namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.AgencyUser
{
    using Common.Constants;
    using Common.Providers;
    using Common.UnitTests.Builders;
    using Common.UnitTests.Mediators;
    using Constants.Messages;
    using FluentAssertions;
    using Manage.Mediators.AgencyUser;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AgencyUserMediatorTests
    {
        [Test]
        public void Authorize_EmptyUsername()
        {
            var mediator = new AgencyUserMediatorBuilder().Build();

            var principal = new ClaimsPrincipalBuilder().Build();

            var response = mediator.Authorize(principal);
            response.AssertMessage(AgencyUserMediatorCodes.Authorize.EmptyUsername, AuthorizeMessages.EmptyUsername, UserMessageLevel.Error);
        }

        [Test]
        public void Authenticated_MissingServicePermission()
        {
            var mediator = new AgencyUserMediatorBuilder().Build();

            var principal = new ClaimsPrincipalBuilder().WithName("User001").Build();

            var response = mediator.Authorize(principal);
            response.AssertMessage(AgencyUserMediatorCodes.Authorize.MissingServicePermission, AuthorizeMessages.MissingServicePermission, UserMessageLevel.Error);
        }

        [Test]
        public void Authenticated_MissingRoleListClaim()
        {
            var mediator = new AgencyUserMediatorBuilder().Build();

            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithRole(Constants.Roles.Raa).Build();

            var response = mediator.Authorize(principal);
            response.AssertMessage(AgencyUserMediatorCodes.Authorize.MissingRoleListClaim, AuthorizeMessages.MissingRoleListClaim, UserMessageLevel.Error);
        }

        [Test]
        public void Authenticated_SessionReturnUrl()
        {
            const string returnUrl = "/localallowedurl";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.ReturnUrl)).Returns(returnUrl);

            var mediator = new AgencyUserMediatorBuilder().With(userDataProvider).Build();

            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithRole(Constants.Roles.Raa).WithRoleList("Agency").Build();

            var response = mediator.Authorize(principal);
            response.AssertCode(AgencyUserMediatorCodes.Authorize.ReturnUrl, false, true);
            response.Parameters.Should().Be(returnUrl);
        }

        [Test]
        public void Authenticated_SessionReturnUrlNotAllowed()
        {
            const string returnUrl = "http://notallowedurl.com/";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.ReturnUrl)).Returns(returnUrl);

            var mediator = new AgencyUserMediatorBuilder().With(userDataProvider).Build();

            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithRole(Constants.Roles.Raa).WithRoleList("Agency").Build();

            var response = mediator.Authorize(principal);
            response.AssertCode(AgencyUserMediatorCodes.Authorize.Ok);
        }

        [Test]
        public void Authenticated_Ok()
        {
            var mediator = new AgencyUserMediatorBuilder().Build();

            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithRole(Constants.Roles.Raa).WithRoleList("Agency").Build();

            var response = mediator.Authorize(principal);
            response.AssertCode(AgencyUserMediatorCodes.Authorize.Ok);
        }
    }
}