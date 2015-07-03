namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    public interface IServiceBusManager
    {
        void Initialise();

        void Subscribe();

        void Unsubscribe();
    }
}