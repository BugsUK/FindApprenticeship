namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.RequestEmailReminderStrategy
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;
    using ErrorCodes = Interfaces.Candidates.ErrorCodes;

    [TestFixture]
    public class RequestEmailReminderStrategyTests
    {
        [Test]
        public void CandidateNotFound()
        {
            const string phoneNumber = "0123456789";

            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(r => r.GetAllCandidatesWithPhoneNumber(phoneNumber, true)).Throws(new CustomException(ErrorCodes.CandidateNotFoundError));
            var communicationService = new Mock<ICommunicationService>();
            var strategy = new RequestEmailReminderStrategyBuilder().With(candidateReadRepository).With(communicationService).Build();

            Action action = () => { strategy.RequestEmailReminder(phoneNumber); };

            action.ShouldThrow<CustomException>().Which.Code.Should().Be(ErrorCodes.CandidateNotFoundError);
            candidateReadRepository.Verify(r => r.GetAllCandidatesWithPhoneNumber(phoneNumber, true), Times.Once);
            candidateReadRepository.Verify(r => r.GetAllCandidatesWithPhoneNumber(phoneNumber, false), Times.Never);
            communicationService.Verify(cs => cs.SendMessageToCandidate(It.IsAny<Guid>(), MessageTypes.SendEmailReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Never);
        }

        [Test]
        public void MobileNumberNotVerified()
        {
            const string phoneNumber = "0123456789";

            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            var candidate = new CandidateBuilder(Guid.NewGuid()).PhoneNumber(phoneNumber).VerifiedMobile(false).Build();
            candidateReadRepository.Setup(r => r.GetAllCandidatesWithPhoneNumber(phoneNumber, true)).Returns(new List<Candidate> { candidate });
            var communicationService = new Mock<ICommunicationService>();
            var strategy = new RequestEmailReminderStrategyBuilder().With(candidateReadRepository).With(communicationService).Build();

            Action action = () => { strategy.RequestEmailReminder(phoneNumber); };

            action.ShouldThrow<CustomException>().Which.Code.Should().Be(Domain.Entities.ErrorCodes.EntityStateError);
            communicationService.Verify(cs => cs.SendMessageToCandidate(It.IsAny<Guid>(), MessageTypes.SendEmailReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Never);
        }

        [Test]
        public void MobileNumberVerified()
        {
            const string phoneNumber = "0123456789";

            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            var candidate = new CandidateBuilder(Guid.NewGuid()).PhoneNumber(phoneNumber).VerifiedMobile(true).Build();
            candidateReadRepository.Setup(r => r.GetAllCandidatesWithPhoneNumber(phoneNumber, true)).Returns(new List<Candidate> { candidate });
            var communicationService = new Mock<ICommunicationService>();
            var strategy = new RequestEmailReminderStrategyBuilder().With(candidateReadRepository).With(communicationService).Build();

            Action action = () => { strategy.RequestEmailReminder(phoneNumber); };

            action.ShouldNotThrow();
            communicationService.Verify(cs => cs.SendMessageToCandidate(It.IsAny<Guid>(), MessageTypes.SendEmailReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Once);
        }

        [Test]
        public void TwoCandidatesNotVerified()
        {
            const string phoneNumber = "0123456789";

            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            var candidateOne = new CandidateBuilder(Guid.NewGuid()).PhoneNumber(phoneNumber).VerifiedMobile(false).Build();
            var candidateTwo = new CandidateBuilder(Guid.NewGuid()).PhoneNumber(phoneNumber).VerifiedMobile(false).Build();
            candidateReadRepository.Setup(r => r.GetAllCandidatesWithPhoneNumber(phoneNumber, true)).Returns(new List<Candidate> { candidateOne, candidateTwo });
            var communicationService = new Mock<ICommunicationService>();
            var strategy = new RequestEmailReminderStrategyBuilder().With(candidateReadRepository).With(communicationService).Build();

            Action action = () => { strategy.RequestEmailReminder(phoneNumber); };

            action.ShouldThrow<CustomException>().Which.Code.Should().Be(Domain.Entities.ErrorCodes.EntityStateError);
            communicationService.Verify(cs => cs.SendMessageToCandidate(It.IsAny<Guid>(), MessageTypes.SendEmailReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Never);
        }

        [Test]
        public void TwoCandidatesVerified()
        {
            const string phoneNumber = "0123456789";

            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            var candidateOne = new CandidateBuilder(Guid.NewGuid()).PhoneNumber(phoneNumber).VerifiedMobile(true).Build();
            var candidateTwo = new CandidateBuilder(Guid.NewGuid()).PhoneNumber(phoneNumber).VerifiedMobile(true).Build();
            candidateReadRepository.Setup(r => r.GetAllCandidatesWithPhoneNumber(phoneNumber, true)).Returns(new List<Candidate> { candidateOne, candidateTwo });
            var communicationService = new Mock<ICommunicationService>();
            var strategy = new RequestEmailReminderStrategyBuilder().With(candidateReadRepository).With(communicationService).Build();

            Action action = () => { strategy.RequestEmailReminder(phoneNumber); };

            action.ShouldNotThrow();
            communicationService.Verify(cs => cs.SendMessageToCandidate(It.IsAny<Guid>(), MessageTypes.SendEmailReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Exactly(2));
        }

        [Test]
        public void TwoCandidatesOneVerified()
        {
            const string phoneNumber = "0123456789";

            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            var candidateOne = new CandidateBuilder(Guid.NewGuid()).PhoneNumber(phoneNumber).VerifiedMobile(false).Build();
            var candidateTwo = new CandidateBuilder(Guid.NewGuid()).PhoneNumber(phoneNumber).VerifiedMobile(true).Build();
            candidateReadRepository.Setup(r => r.GetAllCandidatesWithPhoneNumber(phoneNumber, true)).Returns(new List<Candidate> { candidateOne, candidateTwo });
            var communicationService = new Mock<ICommunicationService>();
            var strategy = new RequestEmailReminderStrategyBuilder().With(candidateReadRepository).With(communicationService).Build();

            Action action = () => { strategy.RequestEmailReminder(phoneNumber); };

            action.ShouldNotThrow();
            communicationService.Verify(cs => cs.SendMessageToCandidate(It.IsAny<Guid>(), MessageTypes.SendEmailReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Once);
        }
    }
}