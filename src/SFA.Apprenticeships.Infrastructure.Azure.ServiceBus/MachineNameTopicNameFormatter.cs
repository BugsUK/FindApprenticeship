namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    using System;

    public class MachineNameTopicNameFormatter : ITopicNameFormatter
    {
        public string GetTopicName(string topicName)
        {
            return $"{Environment.MachineName}-{topicName}";
        }
    }
}