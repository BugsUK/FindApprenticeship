namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes
{
    using System;
    using Application.Candidate;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Infrastructure.Processes.Candidates;
    using Moq;
    using NUnit.Framework;

    public class SaveCandidateRequestConsumerAsyncTests
    {
        private SaveCandidateRequestConsumerAsync _consumer;
        private Mock<IMessageBus> _messageBus;
        private Mock<ICandidateReadRepository> _candidateReadRepository;
        private Mock<ILegacyCandidateProvider> _legacyCandidateProvider;
        private Mock<ILogService> _logger;

        [SetUp]
        public void SetUp()
        {
            _messageBus = new Mock<IMessageBus>();
            _candidateReadRepository = new Mock<ICandidateReadRepository>();
            _legacyCandidateProvider = new Mock<ILegacyCandidateProvider>();
            _logger = new Mock<ILogService>();

            _consumer = new SaveCandidateRequestConsumerAsync(
                _messageBus.Object,
                _candidateReadRepository.Object,
                _legacyCandidateProvider.Object,
                _logger.Object);
        }

        [Test]
        public void ShouldSaveCandidate()
        {
            var candidate = GetCandidate();

            var saveCandidateRequest = new SaveCandidateRequest { CandidateId = Guid.NewGuid() };
            _candidateReadRepository.Setup(x => x.Get(saveCandidateRequest.CandidateId)).Returns(candidate);
            _legacyCandidateProvider.Setup(x => x.UpdateCandidate(candidate));

            var task = _consumer.Consume(saveCandidateRequest);
            task.Wait();
            _candidateReadRepository.Verify(x => x.Get(saveCandidateRequest.CandidateId), Times.Once);
            _legacyCandidateProvider.Verify(x => x.UpdateCandidate(candidate), Times.Once);
        }

        [Test]
        public void ShouldNotRequeueIfCandidateDoesntExixts()
        {
            var candidate = GetCandidate();

            var saveCandidateRequest = new SaveCandidateRequest { CandidateId = Guid.NewGuid() };
            _messageBus.Setup(x => x.PublishMessage(It.IsAny<SaveCandidateRequest>()));
            _candidateReadRepository.Setup(x => x.Get(saveCandidateRequest.CandidateId)).Returns(candidate);
            _legacyCandidateProvider.Setup(x => x.UpdateCandidate(candidate)).Throws(new DomainException(ErrorCodes.CandidateNotFoundError));

            var task = _consumer.Consume(saveCandidateRequest);
            task.Wait();
            _candidateReadRepository.Verify(x => x.Get(saveCandidateRequest.CandidateId), Times.Once);
            _legacyCandidateProvider.Verify(x => x.UpdateCandidate(candidate), Times.Once);
            _messageBus.Verify(x => x.PublishMessage(saveCandidateRequest), Times.Never);
        }

        [Test]
        public void ShouldRequeueIfKnownError()
        {
            var candidate = GetCandidate();

            var saveCandidateRequest = new SaveCandidateRequest { CandidateId = Guid.NewGuid() };
            _messageBus.Setup(x => x.PublishMessage(It.IsAny<SaveCandidateRequest>()));
            _candidateReadRepository.Setup(x => x.Get(saveCandidateRequest.CandidateId)).Returns(candidate);
            _legacyCandidateProvider.Setup(x => x.UpdateCandidate(candidate)).Throws(new Exception());

            var task = _consumer.Consume(saveCandidateRequest);
            task.Wait();
            _candidateReadRepository.Verify(x => x.Get(saveCandidateRequest.CandidateId), Times.Once);
            _legacyCandidateProvider.Verify(x => x.UpdateCandidate(candidate), Times.Once);
            _messageBus.Verify(x => x.PublishMessage(saveCandidateRequest), Times.Once);
        }

        public Candidate GetCandidate()
        {
            return new Candidate
            {
                EntityId = Guid.NewGuid(),
                LegacyCandidateId = 1,
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    EmailAddress = string.Format("nas.exemplar+{0}@gmail.com", Guid.NewGuid()),
                    DateOfBirth = new DateTime(1980, 06, 15),
                    PhoneNumber = "01221234567",
                    Address = new Address
                    {
                        AddressLine1 = "103 Crawley Drive",
                        AddressLine3 = "Hemel Hempstead",
                        AddressLine4 = "Hertfordhsire",
                        Postcode = "HP2 6AL",
                        AddressLine2 = "Hemel Hempstead"
                    },
                }
            };
        }
    }
}
