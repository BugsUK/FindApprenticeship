namespace SFA.Apprenticeships.Infrastructure.Communication.IoC
{
    using System.Collections.Generic;
    using Application.Interfaces.Communications;
    using Common.Configuration;
    using Email;
    using Email.EmailFromResolvers;
    using Email.EmailMessageFormatters;
    using RestSharp;
    using Sms;
    using Sms.SmsMessageFormatters;
    using StructureMap.Configuration.DSL;

    public class CommunicationRegistry : Registry
    {
        public CommunicationRegistry()
        {
            IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>> emailMessageFormatters = new[]
            {
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendActivationCode, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendAccountUnlockCode, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendPasswordResetCode, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.PasswordChanged, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.ApprenticeshipApplicationSubmitted, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.TraineeshipApplicationSubmitted, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.DailyDigest, new EmailDailyDigestMessageFormatter(new ConfigurationManager())),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SavedSearchAlert, new EmailSavedSearchAlertMessageFormatter(new ConfigurationManager())),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.CandidateContactMessage, new EmailSimpleMessageFormatter())
            };

            For<IEmailDispatcher>().Use<SendGridEmailDispatcher>().Named("SendGridEmailDispatcher")
                .Ctor<IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>>>().Is(emailMessageFormatters);

            For<IEmailFromResolver>().Use<CandidateMessageEmailFromResolver>();
            For<IEmailFromResolver>().Use<HelpDeskMessageEmailFromResolver>();

            For<IEmailDispatcher>().Use<VoidEmailDispatcher>().Name = "VoidEmailDispatcher";
            For<ISmsDispatcher>().Use<VoidSmsDispatcher>().Name = "VoidSmsDispatcher";
            For<SendGridConfiguration>().Singleton().Use(SendGridConfiguration.Instance);
            For<IReachSmsConfiguration>().Singleton().Use(ReachSmsConfiguration.Instance);

            IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>> smsMessageFormatters = new[]
            {
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationSubmitted, new SmsApprenticeshipApplicationSubmittedMessageFormatter(ReachSmsConfiguration.Instance.Templates)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.TraineeshipApplicationSubmitted, new SmsTraineeshipApplicationSubmittedMessageFormatter(ReachSmsConfiguration.Instance.Templates)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.SendMobileVerificationCode, new SmsSendMobileVerificationCodeMessageFormatter(ReachSmsConfiguration.Instance.Templates)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationSuccessful, new SmsApprenticeshipApplicationSuccessfulMessageFormatter(ReachSmsConfiguration.Instance.Templates)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationUnsuccessful, new SmsApprenticeshipApplicationUnsuccessfulMessageFormatter(ReachSmsConfiguration.Instance.Templates)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary, new SmsApprenticeshipApplicationsUnsuccessfulSummaryMessageFormatter(ReachSmsConfiguration.Instance.Templates)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationExpiringDraft, new SmsApprenticeshipApplicationExpiringDraftMessageFormatter(ReachSmsConfiguration.Instance.Templates)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary, new SmsApprenticeshipApplicationExpiringDraftsSummaryMessageFormatter(ReachSmsConfiguration.Instance.Templates)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.SavedSearchAlert, new SmsSavedSearchAlertMessageFormatter(ReachSmsConfiguration.Instance.Templates))
            };

            For<ISmsDispatcher>().Use<ReachSmsDispatcher>().Named("ReachSmsDispatcher")
                .Ctor<IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>>>().Is(smsMessageFormatters)
                .Ctor<IRestClient>().Is(new RestClient())
                .Ctor<ISmsNumberFormatter>().Is(new ReachSmsNumberFormatter());
        }
    }
}