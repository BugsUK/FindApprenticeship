namespace SFA.Apprenticeships.Application.UnitTests.Candidate
{
    using System;
    using Apprenticeships.Application.Candidate.Strategies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Infrastructure.Common.Configuration;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class CandidateServiceShould
    {
        [Test]
        public void CallLegacyStrategyIfConfigurationIsLegacy()
        {
            // Arrange
            var candidateId = Guid.NewGuid();
            const int vacancyId = 1;
            var submitLegacyApplicationStrategy = new Mock<ISubmitApprenticeshipApplicationStrategy>();
            var submitRaaApplicationStrategy = new Mock<ISubmitApprenticeshipApplicationStrategy>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(cs => cs.Get<ServicesConfiguration>())
                .Returns(new ServicesConfiguration {ServiceImplementation = ServicesConfiguration.Legacy});

            var candiateService = new CandidateServiceBuilder()
                .With(configurationService)
                .WithSubmitLegacy(submitLegacyApplicationStrategy)
                .WithSubmitRaa(submitRaaApplicationStrategy)
                .Build();

            // Act
            candiateService.SubmitApplication(candidateId, vacancyId);

            // Assert
            submitLegacyApplicationStrategy.Verify(s => s.SubmitApplication(candidateId, vacancyId));
            submitRaaApplicationStrategy.Verify(s => s.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void CallLegacyStrategyIfConfigurationIsRaaAndTheVacancyIsNotMasteredIdRaa()
        {
            // Arrange
            var candidateId = Guid.NewGuid();
            const int vacancyId = 1;
            const bool editedInRaa = false;
            var submitLegacyApplicationStrategy = new Mock<ISubmitApprenticeshipApplicationStrategy>();
            var submitRaaApplicationStrategy = new Mock<ISubmitApprenticeshipApplicationStrategy>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(cs => cs.Get<ServicesConfiguration>())
                .Returns(new ServicesConfiguration { ServiceImplementation = ServicesConfiguration.Raa });

            var candidateApprenticeshipVacancyDetailStrategy =
                new Mock<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>();
            candidateApprenticeshipVacancyDetailStrategy.Setup(s => s.GetVacancyDetails(candidateId, vacancyId))
                .Returns(new ApprenticeshipVacancyDetail
                {
                    EditedInRaa = editedInRaa
                });

            var candiateService = new CandidateServiceBuilder()
                .With(configurationService)
                .WithSubmitLegacy(submitLegacyApplicationStrategy)
                .WithSubmitRaa(submitRaaApplicationStrategy)
                .With(candidateApprenticeshipVacancyDetailStrategy)
                .Build();

            // Act
            candiateService.SubmitApplication(candidateId, vacancyId);

            // Assert
            submitLegacyApplicationStrategy.Verify(s => s.SubmitApplication(candidateId, vacancyId));
            submitRaaApplicationStrategy.Verify(s => s.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void CallRaaStrategyIfConfigurationIsRaaAndTheVacancyIsMasteredIdRaa()
        {
            // Arrange
            var candidateId = Guid.NewGuid();
            const int vacancyId = 1;
            const bool editedInRaa = true;
            var submitLegacyApplicationStrategy = new Mock<ISubmitApprenticeshipApplicationStrategy>();
            var submitRaaApplicationStrategy = new Mock<ISubmitApprenticeshipApplicationStrategy>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(cs => cs.Get<ServicesConfiguration>())
                .Returns(new ServicesConfiguration { ServiceImplementation = ServicesConfiguration.Raa });

            var candidateApprenticeshipVacancyDetailStrategy =
                new Mock<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>();
            candidateApprenticeshipVacancyDetailStrategy.Setup(s => s.GetVacancyDetails(candidateId, vacancyId))
                .Returns(new ApprenticeshipVacancyDetail
                {
                    EditedInRaa = editedInRaa
                });

            var candiateService = new CandidateServiceBuilder()
                .With(configurationService)
                .WithSubmitLegacy(submitLegacyApplicationStrategy)
                .WithSubmitRaa(submitRaaApplicationStrategy)
                .With(candidateApprenticeshipVacancyDetailStrategy)
                .Build();

            // Act
            candiateService.SubmitApplication(candidateId, vacancyId);

            // Assert
            submitRaaApplicationStrategy.Verify(s => s.SubmitApplication(candidateId, vacancyId));
            submitLegacyApplicationStrategy.Verify(s => s.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
        }
    }
}