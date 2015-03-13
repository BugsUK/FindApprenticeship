namespace SFA.Apprenticeships.Infrastructure.Processes.IoC
{
    using Application.Applications;
    using Application.Applications.Strategies;
    using Application.Interfaces.Communications;
    using Applications;
    using Candidates;
    using Communications;
    using Communications.Commands;
    using Microsoft.WindowsAzure;
    using StructureMap.Configuration.DSL;
    using Vacancies;

    public class ProcessesRegistry : Registry
    {
        public ProcessesRegistry()
        {
            // communications
            var emailDispatcher = CloudConfigurationManager.GetSetting("EmailDispatcher");
            var smsDispatcher = CloudConfigurationManager.GetSetting("SmsDispatcher");
            For<EmailRequestConsumerAsync>().Use<EmailRequestConsumerAsync>().Ctor<IEmailDispatcher>().Named(emailDispatcher);
            For<SmsRequestConsumerAsync>().Use<SmsRequestConsumerAsync>().Ctor<ISmsDispatcher>().Named(smsDispatcher);
            For<CommunicationCommand>().Use<CandidateCommunicationCommand>();
            For<CommunicationCommand>().Use<HelpDeskCommunicationCommand>();
            For<CommunicationRequestConsumerAsync>().Use<CommunicationRequestConsumerAsync>();

            // applications
            For<SubmitApprenticeshipApplicationRequestConsumerAsync>().Use<SubmitApprenticeshipApplicationRequestConsumerAsync>();
            For<SubmitTraineeshipApplicationRequestConsumerAsync>().Use<SubmitTraineeshipApplicationRequestConsumerAsync>();
            For<ApplicationStatusChangedConsumerAsync>().Use<ApplicationStatusChangedConsumerAsync>();
            For<IApplicationStatusProcessor>().Use<ApplicationStatusProcessor>();
            For<IApplicationStatusUpdateStrategy>().Use<ApplicationStatusUpdateStrategy>();
            For<IApplicationStatusAlertStrategy>().Use<ApplicationStatusAlertStrategy>();

            // vacancies
            For<VacancyStatusSummaryConsumerAsync>().Use<VacancyStatusSummaryConsumerAsync>();

            // candidates
            For<CandidateSavedSearchesConsumerAsync>().Use<CandidateSavedSearchesConsumerAsync>();
            For<CreateCandidateRequestConsumerAsync>().Use<CreateCandidateRequestConsumerAsync>();
        }
    }
}
