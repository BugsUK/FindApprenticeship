namespace SFA.Apprenticeships.Data.Migrate.Faa.IoC
{
    using Application.Application.Entities;
    using Application.UserAccount.Entities;
    using Domain.Interfaces.Messaging;
    using Infrastructure.Azure.ServiceBus;
    using StructureMap.Configuration.DSL;
    using Subscribers;

    public class FaaMigrationRegistry : Registry
    {
        public FaaMigrationRegistry()
        {
            For<ICandidateUserUpdater>().Use<CandidateUserUpdater>().Singleton();
            For<IApprenticeshipApplicationUpdater>().Use<ApprenticeshipApplicationUpdater>().Singleton();
            For<ITraineeshipApplicationUpdater>().Use<TraineeshipApplicationUpdater>().Singleton();

            // service bus
            RegisterServiceBusMessageBrokers();
        }

        private void RegisterServiceBusMessageBrokers()
        {
            RegisterServiceBusMessageBroker<CandidateUserUpdateSubscriber, CandidateUserUpdate>();
            RegisterServiceBusMessageBroker<ApprenticeshipApplicationUpdateSubscriber, ApprenticeshipApplicationUpdate>();
            RegisterServiceBusMessageBroker<TraineeshipApplicationUpdateSubscriber, TraineeshipApplicationUpdate>();
        }

        private void RegisterServiceBusMessageBroker<TSubscriber, TMessage>() where TSubscriber : IServiceBusSubscriber<TMessage> where TMessage : class
        {
            For<IServiceBusSubscriber<TMessage>>().Use<TSubscriber>();
            For<IServiceBusMessageBroker>().Use<AzureServiceBusMessageBroker<TMessage>>();
        }
    }
}