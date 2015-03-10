namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.IoC
{
    using Application.Applications;
    using Application.Applications.Strategies;
    using Application.Interfaces.Communications;
    using Communications.Commands;
    using Microsoft.WindowsAzure;
    using Consumers;
    using StructureMap.Configuration.DSL;

    public class AsyncProcessorRegistry : Registry
    {
        public AsyncProcessorRegistry()
        {
            var emailDispatcher = CloudConfigurationManager.GetSetting("EmailDispatcher");
            var smsDispatcher = CloudConfigurationManager.GetSetting("SmsDispatcher");

            For<EmailRequestConsumerAsync>().Use<EmailRequestConsumerAsync>().Ctor<IEmailDispatcher>().Named(emailDispatcher);
            For<SmsRequestConsumerAsync>().Use<SmsRequestConsumerAsync>().Ctor<ISmsDispatcher>().Named(smsDispatcher);
            For<SubmitApprenticeshipApplicationRequestConsumerAsync>().Use<SubmitApprenticeshipApplicationRequestConsumerAsync>();
            For<SubmitTraineeshipApplicationRequestConsumerAsync>().Use<SubmitTraineeshipApplicationRequestConsumerAsync>();
            For<CreateCandidateRequestConsumerAsync>().Use<CreateCandidateRequestConsumerAsync>();
            For<ApplicationStatusChangedConsumerAsync>().Use<ApplicationStatusChangedConsumerAsync>();

            For<CommunicationCommand>().Use<CandidateCommunicationCommand>();
            For<CommunicationCommand>().Use<HelpDeskCommunicationCommand>();

            For<CommunicationRequestConsumerAsync>().Use<CommunicationRequestConsumerAsync>();
            For<IApplicationStatusProcessor>().Use<ApplicationStatusProcessor>();
            For<IApplicationStatusUpdateStrategy>().Use<ApplicationStatusUpdateStrategy>();
            For<IApplicationStatusAlertStrategy>().Use<ApplicationStatusAlertStrategy>();
        }
    }
}
