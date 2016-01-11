namespace SFA.Apprenticeships.Infrastructure.Processes.Vacancies
{
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Domain.Interfaces.Messaging;

    public class VacancyStatusProcessorSubscriber : IServiceBusSubscriber<VacancyEligibleForClosure>
    {
        private readonly IVacancyStatusProcessor _vacancyStatusProcessor;

        public VacancyStatusProcessorSubscriber(IVacancyStatusProcessor vacancyStatusProcessor)
        {
            _vacancyStatusProcessor = vacancyStatusProcessor;
        }

        [ServiceBusTopicSubscription(TopicName = "RunVacancyStatusProcessor")]
        public ServiceBusMessageStates Consume(VacancyEligibleForClosure message)
        {
            _vacancyStatusProcessor.ProcessVacancyClosure(message);

            return ServiceBusMessageStates.Complete;
        }
    }
}
