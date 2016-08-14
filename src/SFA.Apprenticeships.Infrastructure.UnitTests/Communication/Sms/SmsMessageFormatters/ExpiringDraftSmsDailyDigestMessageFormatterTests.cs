namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Sms.SmsMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using FluentAssertions;
    using Builders;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ExpiringDraftSmsDailyDigestMessageFormatterTests : SmsDailyDigestMessageFormatterTestsBase
    {
        [Test]
        public void GivenSingleExpiringDraft()
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(1).Build();
            var smsRequest = new DailyDigestSmsRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
            var formatter = new SmsDailyDigestMessageFormatterBuilder().WithMessageTemplate(MessageTemplate).Build();

            var message = formatter.GetMessage(smsRequest.Tokens);

            int draftCount;
            int draftLineCount;
            int alertCount;
            int alertLineCount;
            var expectedMessage = GetExpectedMessage(expiringDrafts, null, out draftCount, out draftLineCount, out alertCount, out alertLineCount);
            message.Should().Be(expectedMessage);
            draftCount.Should().Be(1);
            draftLineCount.Should().Be(1);
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(11)]
        public void GivenMultipleExpiringDrafts(int noOfDrafts)
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(noOfDrafts).Build();
            var smsRequest = new DailyDigestSmsRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
            var formatter = new SmsDailyDigestMessageFormatterBuilder().WithMessageTemplate(MessageTemplate).Build();

            var message = formatter.GetMessage(smsRequest.Tokens);

            int draftCount;
            int draftLineCount;
            int alertCount;
            int alertLineCount;
            var expectedMessage = GetExpectedMessage(expiringDrafts, null, out draftCount, out draftLineCount, out alertCount, out alertLineCount);
            message.Should().Be(expectedMessage);
            draftCount.Should().Be(noOfDrafts);
            draftLineCount.Should().BeLessOrEqualTo(MaxDraftCount);
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(11)]
        public void GivenMultipleExpiringDraftsSpecialCharacters(int noOfDrafts)
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithSpecialCharacterExpiringDrafts(noOfDrafts).Build();
            var smsRequest = new DailyDigestSmsRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();
            var formatter = new SmsDailyDigestMessageFormatterBuilder().WithMessageTemplate(MessageTemplate).Build();

            var message = formatter.GetMessage(smsRequest.Tokens);

            int draftCount;
            int draftLineCount;
            int alertCount;
            int alertLineCount;
            var expectedMessage = GetExpectedMessage(expiringDrafts, null, out draftCount, out draftLineCount, out alertCount, out alertLineCount);
            message.Should().Be(expectedMessage);
            draftCount.Should().Be(noOfDrafts);
            draftLineCount.Should().BeLessOrEqualTo(MaxDraftCount);
        }

        [Test]
        public void GivenMultipleExpiringDrafts_ThenOrderedByClosingDate()
        {
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(3).Build();
            expiringDrafts[0].ClosingDate = new DateTime(2015, 02, 01);
            expiringDrafts[1].ClosingDate = new DateTime(2015, 01, 01);
            expiringDrafts[2].ClosingDate = new DateTime(2015, 04, 01);
            var smsRequest = new DailyDigestSmsRequestBuilder().WithExpiringDrafts(expiringDrafts).Build();

            var draftsJson = smsRequest.Tokens.First(t => t.Key == CommunicationTokens.ExpiringDrafts).Value;
            var drafts = JsonConvert.DeserializeObject<List<ExpiringApprenticeshipApplicationDraft>>(draftsJson);
            Assert.That(drafts, Is.Ordered.By("ClosingDate"));
        }
    }
}