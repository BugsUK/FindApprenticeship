namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public enum ServiceBusMessageStates
    {
        Unknown = 0,
        Complete = 1,
        Requeue = 2,
        Abandon = 3,
        DeadLetter = 4
    }
}