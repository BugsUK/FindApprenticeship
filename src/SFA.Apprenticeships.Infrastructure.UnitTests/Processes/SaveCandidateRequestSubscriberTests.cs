namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes
{
    using System;
    using Application.Candidate;
    using Application.Interfaces.Candidates;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Infrastructure.Processes.Candidates;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;

    public class SaveCandidateRequestSubscriberTests
    {
        private Mock<ILogService> _logger;
        private Mock<ICandidateReadRepository> _candidateReadRepository;
        private Mock<ILegacyCandidateProvider> _legacyCandidateProvider;

        private SaveCandidateRequestSubscriber _subscriber;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogService>();
            _candidateReadRepository = new Mock<ICandidateReadRepository>();
            _legacyCandidateProvider = new Mock<ILegacyCandidateProvider>();

            _subscriber = new SaveCandidateRequestSubscriber(
                _logger.Object,
                _candidateReadRepository.Object,
                _legacyCandidateProvider.Object);
        }

        [Test]
        public void ShouldSaveCandidate()
        {
            // Arrange.
            var candidate = GetCandidate();
            var saveCandidateRequest = new SaveCandidateRequest { CandidateId = Guid.NewGuid() };

            _candidateReadRepository.Setup(x => x.Get(saveCandidateRequest.CandidateId)).Returns(candidate);
            _legacyCandidateProvider.Setup(x => x.UpdateCandidate(candidate));

            // Act.
            var state = _subscriber.Consume(saveCandidateRequest);

            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            _candidateReadRepository.Verify(x => x.Get(saveCandidateRequest.CandidateId), Times.Once);
            _legacyCandidateProvider.Verify(x => x.UpdateCandidate(candidate), Times.Once);
        }

        [Test]
        public void ShouldNotRequeueIfCandidateDoesntExixts()
        {
            // Arrange.
            var candidate = GetCandidate();
            var saveCandidateRequest = new SaveCandidateRequest { CandidateId = Guid.NewGuid() };

            _candidateReadRepository.Setup(x => x.Get(saveCandidateRequest.CandidateId)).Returns(candidate);
            _legacyCandidateProvider.Setup(x => x.UpdateCandidate(candidate)).Throws(new DomainException(ErrorCodes.CandidateNotFoundError));

            // Act.
            var state = _subscriber.Consume(saveCandidateRequest);
            
            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            _candidateReadRepository.Verify(x => x.Get(saveCandidateRequest.CandidateId), Times.Once);
            _legacyCandidateProvider.Verify(x => x.UpdateCandidate(candidate), Times.Once);
        }

        [Test]
        public void ShouldRequeueIfKnownError()
        {
            // Arrange.
            var candidate = GetCandidate();
            var saveCandidateRequest = new SaveCandidateRequest { CandidateId = Guid.NewGuid() };

            _candidateReadRepository.Setup(x => x.Get(saveCandidateRequest.CandidateId)).Returns(candidate);
            _legacyCandidateProvider.Setup(x => x.UpdateCandidate(candidate)).Throws(new Exception());

            // Act.
            var state = _subscriber.Consume(saveCandidateRequest);

            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Requeue);

            _candidateReadRepository.Verify(x => x.Get(saveCandidateRequest.CandidateId), Times.Once);
            _legacyCandidateProvider.Verify(x => x.UpdateCandidate(candidate), Times.Once);
        }

        #region Helpers

        private static Candidate GetCandidate()
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

        #endregion
    }
}
