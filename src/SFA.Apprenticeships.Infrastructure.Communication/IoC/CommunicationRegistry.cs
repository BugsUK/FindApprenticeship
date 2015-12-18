namespace SFA.Apprenticeships.Infrastructure.Communication.IoC
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;
    using Email;
    using Email.EmailFromResolvers;
    using Email.EmailMessageFormatters;
    using RestSharp;
    using Sms;
    using Sms.SmsMessageFormatters;
    using StructureMap;
    using StructureMap.Configuration.DSL;

    public class CommunicationRegistry : Registry
    {
        public CommunicationRegistry()
        {
            For<IEmailDispatcher>().Use<SendGridEmailDispatcher>().Named("SendGridEmailDispatcher")
                .Ctor<IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>>>().Is(context => BuildEmailFormatters(context));

            For<IEmailFromResolver>().Use<CandidateMessageEmailFromResolver>();
            For<IEmailFromResolver>().Use<HelpDeskMessageEmailFromResolver>();

            For<IEmailDispatcher>().Use<VoidEmailDispatcher>().Name = "VoidEmailDispatcher";
            For<ISmsDispatcher>().Use<VoidSmsDispatcher>().Name = "VoidSmsDispatcher";

            For<ISmsDispatcher>().Use<ReachSmsDispatcher>().Named("ReachSmsDispatcher")
                .Ctor<IRestClient>().Is(new RestClient())
                .Ctor<ISmsNumberFormatter>().Is(new ReachSmsNumberFormatter())
                .Ctor<IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>>>().Is(context => BuildSmsFormatters(context));

        }

        private static IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>> BuildEmailFormatters(IContext context)
        {
            var configurationService = context.GetInstance<IConfigurationService>();

            var candidateEmailFormatters = new[]
            {
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendActivationCode, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendAccountUnlockCode, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendPasswordResetCode, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.PasswordChanged, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendPendingUsernameCode, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.ApprenticeshipApplicationSubmitted, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.TraineeshipApplicationSubmitted, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.DailyDigest, new EmailDailyDigestMessageFormatter(configurationService)),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SavedSearchAlert, new EmailSavedSearchAlertMessageFormatter(configurationService)),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.CandidateContactUsMessage, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.CandidateFeedbackMessage, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendActivationCodeReminder, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendDormantAccountReminder, new EmailSimpleMessageFormatter())
            };

            var providerUserEmailFormatters = new[]
            {
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.SendProviderUserEmailVerificationCode, new EmailSimpleMessageFormatter())
            };

            return
                candidateEmailFormatters
                .Union(providerUserEmailFormatters);
        }

        private static IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>> BuildSmsFormatters(IContext context)
        {
            var configurationService = context.GetInstance<IConfigurationService>();
            IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>> smsMessageFormatters = new[]
            {
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationSubmitted, new SmsApprenticeshipApplicationSubmittedMessageFormatter(configurationService)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.TraineeshipApplicationSubmitted, new SmsTraineeshipApplicationSubmittedMessageFormatter(configurationService)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.SendMobileVerificationCode, new SmsSendMobileVerificationCodeMessageFormatter(configurationService)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.SendMobileVerificationCodeReminder, new SmsSendMobileVerificationCodeReminderMessageFormatter(configurationService)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationSuccessful, new SmsApprenticeshipApplicationSuccessfulMessageFormatter(configurationService)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationUnsuccessful, new SmsApprenticeshipApplicationUnsuccessfulMessageFormatter(configurationService)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary, new SmsApprenticeshipApplicationsUnsuccessfulSummaryMessageFormatter(configurationService)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationExpiringDraft, new SmsApprenticeshipApplicationExpiringDraftMessageFormatter(configurationService)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary, new SmsApprenticeshipApplicationExpiringDraftsSummaryMessageFormatter(configurationService)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.SavedSearchAlert, new SmsSavedSearchAlertMessageFormatter(configurationService)),
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.SendEmailReminder, new SmsSendEmailReminderMessageFormatter(configurationService))
            };

            return smsMessageFormatters;
        }
    }
}