namespace SFA.Apprenticeships.Application.UnitTests.Services.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Candidate.Strategies.Apprenticeships;
    using Builders;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Communications;
    using Interfaces.Users;
    using Moq;
    using NUnit.Framework;
    using UnitTests.Candidate.Strategies.SendMobileVerificationCodeStrategy;

    [TestFixture]
    public class SaveCandidateStrategyTests
    {
        [Test]
        public void ShouldUpdateDraftApprenticeshipApplicationDetails()
        {
            var newRegistrationDetails = new RegistrationDetails
            {
                EmailAddress = "updatedEmailAddress@gmail.com"
            };

            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(crr => crr.Get(It.IsAny<Guid>())).Returns(new Candidate{RegistrationDetails = newRegistrationDetails});
            var getCandidateApplicationsStrategy = new Mock<IGetCandidateApprenticeshipApplicationsStrategy>();
            getCandidateApplicationsStrategy.Setup(gca => gca.GetApplications(It.IsAny<Guid>(), true))
                .Returns(new[] {new ApprenticeshipApplicationSummary {Status = ApplicationStatuses.Draft}});
            var apprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            apprenticeshipApplicationReadRepository.Setup(
                aprr => aprr.GetForCandidate(It.IsAny<Guid>(), It.IsAny<int>(),false))
                .Returns(new ApprenticeshipApplicationDetail());
            var apprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();

            var saveCandidateStrategy = new SaveCandidateStrategyBuilder().With(candidateReadRepository).With(getCandidateApplicationsStrategy).With(apprenticeshipApplicationReadRepository).With(apprenticeshipApplicationWriteRepository).Build();

            saveCandidateStrategy.SaveCandidate(new Candidate());

            apprenticeshipApplicationWriteRepository.Verify(
                aawr =>
                    aawr.Save(It.Is<ApprenticeshipApplicationDetail>(a => a.CandidateDetails.EmailAddress == newRegistrationDetails.EmailAddress)));
        }

        [Test]
        public void ShouldNotUpdateNoDraftApprenticeshipApplicationDetails()
        {
            var newRegistrationDetails = new RegistrationDetails
            {
                EmailAddress = "updatedEmailAddress@gmail.com"
            };

            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(crr => crr.Get(It.IsAny<Guid>())).Returns(new Candidate { RegistrationDetails = newRegistrationDetails });
            var getCandidateApplicationsStrategy = new Mock<IGetCandidateApprenticeshipApplicationsStrategy>();
            getCandidateApplicationsStrategy.Setup(gca => gca.GetApplications(It.IsAny<Guid>(), true))
                .Returns(new[]
                {
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.Successful },
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.ExpiredOrWithdrawn },
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.InProgress },
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.Submitted },
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.Submitting },
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.Unknown },
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.Unsuccessful }
                });
            var apprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            apprenticeshipApplicationReadRepository.Setup(
                aprr => aprr.GetForCandidate(It.IsAny<Guid>(), It.IsAny<int>(), false))
                .Returns(new ApprenticeshipApplicationDetail());
            var apprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();

            var saveCandidateStrategy = new SaveCandidateStrategyBuilder().With(candidateReadRepository).With(getCandidateApplicationsStrategy).With(apprenticeshipApplicationReadRepository).Build();

            saveCandidateStrategy.SaveCandidate(new Candidate());

            apprenticeshipApplicationWriteRepository.Verify(
                aawr =>
                    aawr.Save(It.Is<ApprenticeshipApplicationDetail>(a => a.CandidateDetails.EmailAddress == newRegistrationDetails.EmailAddress)), Times.Never());

        }

        [TestCase(true, false)]
        [TestCase(true, true)]
        [TestCase(false, false)]
        [TestCase(false, true)]
        public void ShouldAssignAndSendMobileUserCodeIfVerificationIsRequired(bool verifiedMobile, bool enableText)
        {
            var candidateId = Guid.NewGuid();
            const string phoneNumber = "0123456789";
            var candidate = new Candidate
            {
                EntityId = candidateId,
                RegistrationDetails = new RegistrationDetails
                {
                    PhoneNumber = phoneNumber
                },
                CommunicationPreferences = new CommunicationPreferences
                {
                    ApplicationStatusChangePreferences = new CommunicationPreference
                    {
                        EnableText = enableText
                    },
                    ExpiringApplicationPreferences = new CommunicationPreference
                    {
                        EnableText = enableText
                    },
                    SavedSearchPreferences = new CommunicationPreference
                    {
                        EnableText = enableText
                    },
                    MarketingPreferences = new CommunicationPreference
                    {
                        EnableText = enableText
                    },
                    VerifiedMobile = verifiedMobile
                }
            };

            var codeGenerator = new Mock<ICodeGenerator>();
            const string mobileVerificationCode = "1234";
            
            codeGenerator.Setup(cg => cg.GenerateNumeric(4)).Returns(mobileVerificationCode);
            
            var communicationService = new Mock<ICommunicationService>();
            
            IEnumerable<CommunicationToken> communicationTokens = new List<CommunicationToken>(0);

            communicationService.Setup(cs => cs.SendMessageToCandidate(candidateId, MessageTypes.SendMobileVerificationCode, It.IsAny<IEnumerable<CommunicationToken>>())).Callback<Guid, MessageTypes, IEnumerable<CommunicationToken>>((cid, mt, ct) => { communicationTokens = ct; });

            var sendMobileVerificationCodeStrategy = new SendMobileVerificationCodeStrategyBuilder().With(communicationService).With(codeGenerator).Build();
            var saveCandidateStrategy = new SaveCandidateStrategyBuilder().With(sendMobileVerificationCodeStrategy).Build();

            saveCandidateStrategy.SaveCandidate(candidate);
            if (candidate.MobileVerificationRequired())
            {
                sendMobileVerificationCodeStrategy.SendMobileVerificationCode(candidate);
            }

            if (verifiedMobile || !enableText)
            {
                candidate.CommunicationPreferences.MobileVerificationCode.Should().BeNullOrEmpty();
                candidate.CommunicationPreferences.MobileVerificationCodeDateCreated.Should().NotHaveValue();
                communicationService.Verify(cs => cs.SendMessageToCandidate(candidateId, MessageTypes.SendMobileVerificationCode, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Never);
            }
            else
            {
                candidate.CommunicationPreferences.MobileVerificationCode.Should().Be(mobileVerificationCode);
                candidate.CommunicationPreferences.MobileVerificationCodeDateCreated.Should().HaveValue();
                // ReSharper disable once PossibleInvalidOperationException
                candidate.CommunicationPreferences.MobileVerificationCodeDateCreated.Value.Should().BeCloseTo(DateTime.UtcNow, 500);

                communicationService.Verify(cs => cs.SendMessageToCandidate(candidateId, MessageTypes.SendMobileVerificationCode, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Once);

                var communicationTokensList = communicationTokens.ToList();

                communicationTokensList.Count.Should().Be(2);
                communicationTokensList.Single(ct => ct.Key == CommunicationTokens.CandidateMobileNumber).Value.Should().Be(phoneNumber);
                communicationTokensList.Single(ct => ct.Key == CommunicationTokens.MobileVerificationCode).Value.Should().Be(mobileVerificationCode);
            }
        }
    }
}