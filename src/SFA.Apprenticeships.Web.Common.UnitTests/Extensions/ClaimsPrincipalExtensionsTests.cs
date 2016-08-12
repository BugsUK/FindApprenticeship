namespace SFA.Apprenticeships.Web.Common.UnitTests.Extensions
{
    using Builders;
    using Common.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ClaimsPrincipalExtensionsTests
    {
        private const string Group = "group";

        [Test]
        public void GivenAUserWithoutGroupClaim_IsInGroupShouldReturnFalse()
        {
            var user = new ClaimsPrincipalBuilder().Build();

            user.IsInGroup(Group).Should().BeFalse();
        }

        [Test]
        public void GivenAUserWithAnotherGroupClaim_IsInGroupShouldReturnFalse()
        {
            var user = new ClaimsPrincipalBuilder().WithGroup("anotherGroup").Build();

            user.IsInGroup(Group).Should().BeFalse();
        }

        [Test]
        public void GivenAUserWithTheGroupClaim_IsInGroupShouldReturnTrue()
        {
            var user = new ClaimsPrincipalBuilder().WithGroup(Group).Build();

            user.IsInGroup(Group).Should().BeTrue();
        }
    }
}