namespace SFA.Apprenticeships.Application.UnitTests.Applications.Extensions
{
    using System;
    using Apprenticeships.Application.Applications.Entities;
    using Apprenticeships.Application.Applications.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ApplicationStatusSummaryExtensionsTests
    {
        [Test]
        public void ShouldIdentifyLegacySystemUpdate()
        {
            // Arrange.
            var summary = new ApplicationStatusSummary
            {
                ApplicationId = Guid.Empty
            };

            // Act.
            var actual = summary.IsLegacySystemUpdate();

            // Assert.
            actual.Should().BeTrue();
        }

        [Test]
        public void ShouldIdentifyNonLegacySystemUpdate()
        {
            // Arrange.
            var summary = new ApplicationStatusSummary
            {
                ApplicationId = Guid.NewGuid()
            };

            // Act.
            var actual = summary.IsLegacySystemUpdate();

            // Assert.
            actual.Should().BeFalse();
        }
    }
}
