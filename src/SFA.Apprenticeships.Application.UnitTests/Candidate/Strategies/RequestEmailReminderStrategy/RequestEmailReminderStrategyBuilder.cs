namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.RequestEmailReminderStrategy
{
    using Apprenticeships.Application.Candidate.Strategies;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Moq;

    public class RequestEmailReminderStrategyBuilder
    {
        private Mock<ICandidateReadRepository> _candidateReadRepository = new Mock<ICandidateReadRepository>();
        private Mock<ICommunicationService> _communicationService = new Mock<ICommunicationService>();

        public IRequestEmailReminderStrategy Build()
        {
            var strategy = new RequestEmailReminderStrategy(_candidateReadRepository.Object, _communicationService.Object);
            return strategy;
        }

        public RequestEmailReminderStrategyBuilder With(Mock<ICandidateReadRepository> candidateReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
            return this;
        }

        public RequestEmailReminderStrategyBuilder With(Mock<ICommunicationService> communicationService)
        {
            _communicationService = communicationService;
            return this;
        }
    }
}