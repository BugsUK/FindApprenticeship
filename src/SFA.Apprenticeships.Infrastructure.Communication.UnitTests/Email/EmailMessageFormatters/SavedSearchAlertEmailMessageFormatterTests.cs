namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Email.EmailMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Builder;
    using Builders;
    using Domain.Entities.Communication;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Helpers;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SendGrid;

    [TestFixture]
    public class SavedSearchAlertEmailMessageFormatterTests
    {
        protected const string CandidateFirstNameTag = "-Candidate.FirstName-";
        protected const string SavedSearchAlertsTag = "-Saved.Search.Alerts-";

        [Test]
        public void ShouldSubstituteCandidateFirstName()
        {
            // Arrange.
            var savedSearchAlerts = new SavedSearchAlertsBuilder().Build();
            var request = new SavedSearchAlertEmailRequestBuilder().WithSavedSearchAlerts(savedSearchAlerts).Build();

            List<SendGridMessageSubstitution> substitutions;

            var message = GetMockSendGridMessage(out substitutions);
            var formatter = new EmailSavedSearchAlertMessageFormatterBuilder().Build();

            // Act.
            formatter.PopulateMessage(request, message.Object);

            // Assert.
            var substitution = substitutions.FirstOrDefault(s => s.ReplacementTag == CandidateFirstNameTag);

            substitution.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            substitution.SubstitutionValues.Count.Should().Be(1);
        }

        [Test]
        public void ShouldSubsituteSavedSearchAlerts()
        {
            // Arrange.
            var savedSearchAlerts = new SavedSearchAlertsBuilder().Build();
            var emailRequest = new SavedSearchAlertEmailRequestBuilder().WithSavedSearchAlerts(savedSearchAlerts).Build();

            List<SendGridMessageSubstitution> substitutions;

            var message = GetMockSendGridMessage(out substitutions);
            var formatter = new EmailSavedSearchAlertMessageFormatterBuilder().Build();

            // Act.
            formatter.PopulateMessage(emailRequest, message.Object);

            // Assert.
            var substitution = substitutions.FirstOrDefault(s => s.ReplacementTag == SavedSearchAlertsTag);

            substitution.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            substitution.SubstitutionValues.Count.Should().Be(1);
        }

        [Test]
        public void ShouldFormatSingleSavedSearchAlert()
        {
            // Arrange.
            var savedSearchAlerts = new Fixture()
                .Build<SavedSearchAlert>()
                .CreateMany(1)
                .ToList();

            var request = new SavedSearchAlertEmailRequestBuilder()
                .WithSavedSearchAlerts(savedSearchAlerts)
                .Build();

            List<SendGridMessageSubstitution> substitutions;

            var message = GetMockSendGridMessage(out substitutions);
            var formatter = new EmailSavedSearchAlertMessageFormatterBuilder().Build();

            // Act.
            formatter.PopulateMessage(request, message.Object);

            // Assert.
            var substitution = substitutions.FirstOrDefault(s => s.ReplacementTag == SavedSearchAlertsTag);

            substitution.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            substitution.SubstitutionValues.Count.Should().Be(1);

            var fragment = substitution.SubstitutionValues.First();

            fragment.Should().NotBeEmpty();
        }

        [Test]
        public void ShouldFormatMultipleSavedSearchAlert()
        {
            // Arrange.
            var savedSearchAlerts = new Fixture()
                .Build<SavedSearchAlert>()
                .CreateMany(5)
                .ToList();

            var request = new SavedSearchAlertEmailRequestBuilder()
                .WithSavedSearchAlerts(savedSearchAlerts)
                .Build();

            List<SendGridMessageSubstitution> substitutions;

            var message = GetMockSendGridMessage(out substitutions);
            var formatter = new EmailSavedSearchAlertMessageFormatterBuilder().Build();

            // Act.
            formatter.PopulateMessage(request, message.Object);

            // Assert.
            var substitution = substitutions.FirstOrDefault(s => s.ReplacementTag == SavedSearchAlertsTag);

            substitution.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            substitution.SubstitutionValues.Count.Should().Be(1);

            var fragment = substitution.SubstitutionValues.First();

            fragment.Should().NotBeEmpty();
        }

        #region Helpers

        protected static Mock<ISendGrid> GetMockSendGridMessage(out List<SendGridMessageSubstitution> sendGridMessageSubstitutions)
        {
            var sendGridMessage = new Mock<ISendGrid>();
            var substitutions = new List<SendGridMessageSubstitution>();

            sendGridMessage
                .Setup(m => m.AddSubstitution(It.IsAny<string>(), It.IsAny<List<string>>()))
                .Callback<string, List<string>>((replacementTag, substitutionValues) => substitutions.Add(new SendGridMessageSubstitution(replacementTag, substitutionValues)));

            sendGridMessageSubstitutions = substitutions;

            return sendGridMessage;
        }

        #endregion
    }
}
