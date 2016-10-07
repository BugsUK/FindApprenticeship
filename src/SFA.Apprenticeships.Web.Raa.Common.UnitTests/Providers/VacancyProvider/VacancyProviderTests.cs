namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces;
    using Application.Interfaces.Providers;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories.Models;
    using FluentAssertions;
    using Moq;
    using Moq.Language.Flow;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Web.Common.Configuration;

    [TestFixture]
    [Parallelizable]
    public class VacancyProviderTests
    {
        private const int QAVacancyTimeout = 10;
        private const int ProviderId = 2;
        

        [Test]
        public void GetVacanciesPendingQAShouldCallRepositoryWithPendingQAAsDesiredStatus()
        {
            //Arrange
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration {BlacklistedCategoryCodes = ""});

            int total;

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(It.IsAny<VacancySummaryByStatusQuery>(), out total))
                .Returns(new List<VacancySummary>
                {
                    new VacancySummary
                    {
                        ClosingDate = DateTime.Now,
                        DateSubmitted = DateTime.Now,
                        ProviderId = ProviderId,
                        Status = VacancyStatus.Submitted
                    }
                });

            var metrics = new List<RegionalTeamMetrics>()
            {
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.EastAnglia },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.EastMidlands },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.North },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.NorthWest },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.SouthEast },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.SouthWest },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.WestMidlands },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.YorkshireAndHumberside },
            };

            vacancyPostingService.Setup(p => p.GetRegionalTeamsMetrics(It.IsAny<VacancySummaryByStatusQuery>())).Returns(metrics);

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .With(configurationService)
                    .Build();

            //Act
            vacancyProvider.GetPendingQAVacancies();

            //Assert
            vacancyPostingService.Verify(avr => avr.GetWithStatus(It.IsAny<VacancySummaryByStatusQuery>(), out total));
            vacancyPostingService.Verify(avr => avr.GetRegionalTeamsMetrics(It.IsAny<VacancySummaryByStatusQuery>()));
        }
    }

    public static class MoqExtensions
    {
        public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup,
            params TResult[] results) where T : class
        {
            setup.Returns(new Queue<TResult>(results).Dequeue);
        }
    }
}