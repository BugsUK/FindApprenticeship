namespace SFA.Apprenticeships.Application.UnitTests.Applications.Extensions
{
    using Apprenticeships.Application.Application.Entities;
    using Apprenticeships.Application.Application.Extensions;
    using FluentAssertions;
    using NUnit.Framework;
    using System;

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
