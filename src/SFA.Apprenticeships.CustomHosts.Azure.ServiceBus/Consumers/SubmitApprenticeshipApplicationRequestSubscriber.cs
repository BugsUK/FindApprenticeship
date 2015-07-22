namespace SFA.Apprenticeships.CustomHosts.Azure.ServiceBus.Consumers
{
    using System;
    using Application.Candidate;
    using Domain.Interfaces.Messaging;

    public class SubmitApprenticeshipApplicationRequestSubscriber : IServiceBusSubscriber<SubmitApprenticeshipApplicationRequest>
    {
        [ServiceBusTopicSubscription(TopicName = "aship-application-submit")]
        public ServiceBusMessageResult Consume(SubmitApprenticeshipApplicationRequest message)
        {
            Console.WriteLine("DEFAULT: SubmitApprenticeshipApplicationRequest: {0}", message.ApplicationId);
            return ServiceBusMessageResult.Complete();
        }
    }
}