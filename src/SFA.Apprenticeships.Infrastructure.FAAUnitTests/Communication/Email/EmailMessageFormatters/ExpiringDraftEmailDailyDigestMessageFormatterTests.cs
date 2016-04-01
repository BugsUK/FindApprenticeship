namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Email.EmailMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using Communication.Builders;
    using Builders;
    using Helpers;
    using FluentAssertions;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class ExpiringDraftEmailDailyDigestMessageFormatterTests : EmailDailyDigestMessageFormatterTestsBase
    {
        [Test]
        public void GivenSingleExpiringDraft()
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(1).Build();
            var emailRequest = new DailyDigestEmailRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatterBuilder().Build();
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            sendGridMessageSubstitutions.Count(s => s.ReplacementTag == ExpiringDraftsTag).Should().Be(1);
            var expiringDraftsSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ExpiringDraftsTag);
            expiringDraftsSubstitution.SubstitutionValues.Single().Should().Be(GetExpectedInfoSubstitution(expiringDrafts));
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(11)]
        public void GivenMultipleExpiringDrafts(int noOfDrafts)
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(noOfDrafts).Build();
            var emailRequest = new DailyDigestEmailRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatterBuilder().Build();
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            sendGridMessageSubstitutions.Count(s => s.ReplacementTag == ExpiringDraftsTag).Should().Be(1);
            var expiringDraftsSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ExpiringDraftsTag);
            expiringDraftsSubstitution.SubstitutionValues.Single().Should().Be(GetExpectedInfoSubstitution(expiringDrafts));
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(11)]
        public void GivenMultipleExpiringDraftsSpecialCharacters(int noOfDrafts)
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithSpecialCharacterExpiringDrafts(noOfDrafts).Build();
            var emailRequest = new DailyDigestEmailRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatterBuilder().Build();
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            sendGridMessageSubstitutions.Count(s => s.ReplacementTag == ExpiringDraftsTag).Should().Be(1);
            var expiringDraftsSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ExpiringDraftsTag);
            expiringDraftsSubstitution.SubstitutionValues.Single().Should().Be(GetExpectedInfoSubstitution(expiringDrafts));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void ShouldContainCandidateFirstNameSubstitution(int noOfDrafts)
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithSpecialCharacterExpiringDrafts(noOfDrafts).Build();
            var emailRequest = new DailyDigestEmailRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatterBuilder().Build();
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            sendGridMessageSubstitutions.Any(s => s.ReplacementTag == CandidateFirstNameTag).Should().BeTrue();
        }

        [Test]
        public void GivenMultipleExpiringDrafts_ThenOrderedByClosingDate()
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(3).Build();
            expiringDrafts[0].ClosingDate = new DateTime(2015, 02, 01);
            expiringDrafts[1].ClosingDate = new DateTime(2015, 01, 01);
            expiringDrafts[2].ClosingDate = new DateTime(2015, 04, 01);
            var emailRequest = new DailyDigestEmailRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();

            var draftsJson = emailRequest.Tokens.First(t => t.Key == CommunicationTokens.ExpiringDrafts).Value;
            var drafts = JsonConvert.DeserializeObject<List<ExpiringApprenticeshipApplicationDraft>>(draftsJson);
            Assert.That(drafts, Is.Ordered.By("ClosingDate"));
        }
    }
}
