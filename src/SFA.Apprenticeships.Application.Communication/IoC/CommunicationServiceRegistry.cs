namespace SFA.Apprenticeships.Application.Communication.IoC
{
    using Strategies;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class CommunicationServiceRegistry : Registry
    {
        public CommunicationServiceRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

            For<ISendApplicationSubmittedStrategy>().Use<QueueApprenticeshipApplicationSubmittedStrategy>();
            For<ISendCandidateCommunicationStrategy>().Use<QueueCandidateCommunicationStrategy>();
            For<ISendContactMessageStrategy>().Use<QueueContactMessageStrategy>();
            For<ISendEmployerCommunicationStrategy>().Use<QueueEmployerCommunicationStrategy>();
            For<ISendProviderUserCommunicationStrategy>().Use<QueueProviderUserCommunicationStrategy>();
            For<ISendTraineeshipApplicationSubmittedStrategy>().Use<QueueTraineeshipApplicationSubmittedStrategy>();
            For<ISendUsernameUpdateCommunicationStrategy>().Use<QueueUsernameUpdateCommunicationStrategy>();
        }
    }
}