namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Sms.SmsMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using FluentAssertions;
    using Builders;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ApplicationStatusAlertSmsDailyDigestMessageFormatterTests : SmsDailyDigestMessageFormatterTestsBase
    {
        [TestCase(ApplicationStatuses.Unsuccessful)]
        [TestCase(ApplicationStatuses.Successful)]
        public void GivenSingleApplicationStatusAlert(ApplicationStatuses applicationStatus)
        {
            var alerts = new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(1, applicationStatus).Build();
            var smsRequest = new DailyDigestSmsRequestBuilder().WithApplicationStatusAlerts(alerts).Build();
            var formatter = new SmsDailyDigestMessageFormatterBuilder().WithMessageTemplate(MessageTemplate).Build();

            var message = formatter.GetMessage(smsRequest.Tokens);

            int draftCount;
            int draftLineCount;
            int alertCount;
            int alertLineCount;
            var expectedMessage = GetExpectedMessage(null, alerts, out draftCount, out draftLineCount, out alertCount, out alertLineCount);
            message.Should().Be(expectedMessage);
            draftCount.Should().Be(0);
            draftLineCount.Should().Be(0);
            alertCount.Should().Be(1);
            alertLineCount.Should().Be(1);
        }

        [Test]
        public void GivenMultipleExpiringApplicationStatusAlerts()
        {
            var alerts = new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(2, ApplicationStatuses.Successful).Build()
                .Concat(new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(3, ApplicationStatuses.Unsuccessful).Build()).ToList();
            var smsRequest = new DailyDigestSmsRequestBuilder().WithApplicationStatusAlerts(alerts).Build();
            var formatter = new SmsDailyDigestMessageFormatterBuilder().WithMessageTemplate(MessageTemplate).Build();

            var message = formatter.GetMessage(smsRequest.Tokens);

            int draftCount;
            int draftLineCount;
            int alertCount;
            int alertLineCount;
            var expectedMessage = GetExpectedMessage(null, alerts, out draftCount, out draftLineCount, out alertCount, out alertLineCount);
            message.Should().Be(expectedMessage);
            draftCount.Should().Be(0);
            draftLineCount.Should().BeLessOrEqualTo(MaxDraftCount);
            alertCount.Should().Be(5);
            alertLineCount.Should().Be(3);
        }

        [Test]
        public void GivenMultipleExpiringApplicationStatusAlertsAndExpiringDrafts()
        {
            var alerts = new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(2, ApplicationStatuses.Successful).Build()
                .Concat(new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(3, ApplicationStatuses.Unsuccessful).Build()).ToList();
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(5).Build();
            var smsRequest = new DailyDigestSmsRequestBuilder().WithApplicationStatusAlerts(alerts).WithExpiringDrafts(expiringDrafts).Build();
            var formatter = new SmsDailyDigestMessageFormatterBuilder().WithMessageTemplate(MessageTemplate).Build();

            var message = formatter.GetMessage(smsRequest.Tokens);

            int draftCount;
            int draftLineCount;
            int alertCount;
            int alertLineCount;
            var expectedMessage = GetExpectedMessage(expiringDrafts, alerts, out draftCount, out draftLineCount, out alertCount, out alertLineCount);
            message.Should().Be(expectedMessage);
            draftCount.Should().Be(5);
            draftLineCount.Should().Be(3);
            alertCount.Should().Be(5);
            alertLineCount.Should().Be(3);
        }

        [Test]
        public void GivenMultipleApplicationStatusAlerts_ThenOrderedByDateUpdated()
        {
            var alerts = new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(1, ApplicationStatuses.Successful).Build()
                .Concat(new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(2, ApplicationStatuses.Unsuccessful).Build()).ToList();
            alerts[0].DateUpdated = new DateTime(2015, 02, 01);
            alerts[1].DateUpdated = new DateTime(2015, 01, 01);
            alerts[2].DateUpdated = new DateTime(2015, 04, 01);
            var smsRequest = new DailyDigestSmsRequestBuilder().WithApplicationStatusAlerts(alerts).Build();

            var alertsJson = smsRequest.Tokens.First(t => t.Key == CommunicationTokens.ApplicationStatusAlerts).Value;
            var applicationStatusAlerts = JsonConvert.DeserializeObject<List<ExpiringApprenticeshipApplicationDraft>>(alertsJson);
            Assert.That(applicationStatusAlerts, Is.Ordered.By("DateUpdated"));
        }
    }
}