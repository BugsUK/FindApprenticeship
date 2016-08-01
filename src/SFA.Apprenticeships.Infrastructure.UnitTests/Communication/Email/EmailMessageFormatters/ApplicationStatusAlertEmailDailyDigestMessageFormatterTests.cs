namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Email.EmailMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using FluentAssertions;
    using Communication.Builders;
    using Builders;
    using Helpers;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ApplicationStatusAlertEmailDailyDigestMessageFormatterTests : EmailDailyDigestMessageFormatterTestsBase
    {
        [TestCase(ApplicationStatuses.Unsuccessful)]
        [TestCase(ApplicationStatuses.Successful)]
        public void GivenSingleApplicationStatusAlert(ApplicationStatuses applicationStatus)
        {
            var alerts = new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(1, applicationStatus).Build();
            var emailRequest = new DailyDigestEmailRequestBuilder().WithApplicationStatusAlerts(alerts).Build();
            
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatterBuilder().Build();
            
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            var applicationStatusAlertTagSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ApplicationStatusAlertTag);

            applicationStatusAlertTagSubstitution.SubstitutionValues.Count.Should().Be(1);
            applicationStatusAlertTagSubstitution.SubstitutionValues.Single().Should().Be(GetExpectedInfoSubstitution(alerts));
        }

        [Test]
        public void GivenMultipleExpiringApplicationStatusAlerts()
        {
            var alerts = new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(2, ApplicationStatuses.Successful).Build()
                .Concat(new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(3, ApplicationStatuses.Unsuccessful).Build()).ToList();
            var emailRequest = new DailyDigestEmailRequestBuilder().WithApplicationStatusAlerts(alerts).Build();
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatterBuilder().Build();
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            var applicationStatusAlertTagSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ApplicationStatusAlertTag);
            applicationStatusAlertTagSubstitution.SubstitutionValues.Count.Should().Be(1);
            applicationStatusAlertTagSubstitution.SubstitutionValues.Single().Should().Be(GetExpectedInfoSubstitution(alerts));
        }

        [Test]
        public void GivenMultipleExpiringApplicationStatusAlertsAndExpiringDrafts()
        {
            var alerts = new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(2, ApplicationStatuses.Successful).Build()
                .Concat(new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(3, ApplicationStatuses.Unsuccessful).Build()).ToList();
            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder().WithExpiringDrafts(3).Build();
            var emailRequest = new DailyDigestEmailRequestBuilder().WithApplicationStatusAlerts(alerts).WithExpiringDrafts(expiringDrafts).Build();
            List<SendGridMessageSubstitution> sendGridMessageSubstitutions;
            var sendGridMessage = GetSendGridMessage(out sendGridMessageSubstitutions);

            var emailMessageFormatter = new EmailDailyDigestMessageFormatterBuilder().Build();
            emailMessageFormatter.PopulateMessage(emailRequest, sendGridMessage.Object);

            var applicationStatusAlertTagSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ApplicationStatusAlertTag);
            applicationStatusAlertTagSubstitution.SubstitutionValues.Count.Should().Be(1);
            applicationStatusAlertTagSubstitution.SubstitutionValues.Single().Should().Be(GetExpectedInfoSubstitution(alerts));
            sendGridMessageSubstitutions.Count(s => s.ReplacementTag == ExpiringDraftsTag).Should().Be(1);
            var expiringDraftsSubstitution = sendGridMessageSubstitutions.Single(s => s.ReplacementTag == ExpiringDraftsTag);
            expiringDraftsSubstitution.SubstitutionValues.Single().Should().Be(GetExpectedInfoSubstitution(expiringDrafts));
        }

        [Test]
        public void GivenMultipleApplicationStatusAlerts_ThenOrderedByDateUpdated()
        {
            var alerts = new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(1, ApplicationStatuses.Successful).Build()
                .Concat(new ApplicationStatusAlertsBuilder().WithApplicationStatusAlerts(2, ApplicationStatuses.Unsuccessful).Build()).ToList();
            alerts[0].DateUpdated = new DateTime(2015, 02, 01);
            alerts[1].DateUpdated = new DateTime(2015, 01, 01);
            alerts[2].DateUpdated = new DateTime(2015, 04, 01);
            var emailRequest = new DailyDigestEmailRequestBuilder().WithApplicationStatusAlerts(alerts).Build();

            var alertsJson = emailRequest.Tokens.First(t => t.Key == CommunicationTokens.ApplicationStatusAlerts).Value;
            var applicationStatusAlerts = JsonConvert.DeserializeObject<List<ExpiringApprenticeshipApplicationDraft>>(alertsJson);
            Assert.That(applicationStatusAlerts, Is.Ordered.By("DateUpdated"));
        }
    }
}
