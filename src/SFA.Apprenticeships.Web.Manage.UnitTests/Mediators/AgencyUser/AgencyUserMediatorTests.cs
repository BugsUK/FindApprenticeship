namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.AgencyUser
{
    using Common.Constants;
    using Common.UnitTests.Builders;
    using Common.UnitTests.Mediators;
    using Constants.Messages;
    using Manage.Mediators.AgencyUser;
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
    }
}