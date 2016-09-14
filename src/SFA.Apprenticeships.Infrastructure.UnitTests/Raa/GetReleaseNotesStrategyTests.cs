namespace SFA.Apprenticeships.Infrastructure.UnitTests.Raa
{
    using System.Collections.Generic;
    using Domain.Entities.ReferenceData;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Infrastructure.Raa.Strategies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetReleaseNotesStrategyTests
    {
        [TestCase(DasApplication.Find)]
        [TestCase(DasApplication.Manage)]
        [TestCase(DasApplication.Recruit)]
        public void GetForDasApplication(DasApplication dasApplication)
        {
            var referenceRepository = new Mock<IReferenceRepository>();
            referenceRepository.Setup(r => r.GetReleaseNotes()).Returns(new List<ReleaseNote>
            {
                new ReleaseNote(DasApplication.Find, 1, "Find"),
                new ReleaseNote(DasApplication.Manage, 1, "Manage"),
                new ReleaseNote(DasApplication.Recruit, 1, "Recruit"),
                new ReleaseNote((DasApplication)0x07, 2, "All")
            });
            var getReleaseNotesStrategy = new GetReleaseNotesStrategy(referenceRepository.Object);

            var releaseNotes = getReleaseNotesStrategy.GetReleaseNotes(dasApplication);

            releaseNotes.Count.Should().Be(2);
            releaseNotes[0].Application.Should().Be(dasApplication);
            releaseNotes[0].Version.Should().Be(1);
            releaseNotes[0].Note.Should().Be(dasApplication.ToString());
            releaseNotes[1].Application.Should().Be(DasApplication.Find | DasApplication.Manage | DasApplication.Recruit);
            releaseNotes[1].Version.Should().Be(2);
            releaseNotes[1].Note.Should().Be("All");
        }
    }
}