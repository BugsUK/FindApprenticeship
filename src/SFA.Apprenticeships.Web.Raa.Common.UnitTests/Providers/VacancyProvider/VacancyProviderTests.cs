namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Configuration;
    using Domain.Entities.Raa.Parties; 
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using Moq.Language.Flow;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.Vacancy;
    using Web.Common.Configuration;
    using Web.Common.ViewModels;

    [TestFixture]
    public class VacancyProviderTests
    {
        private const int QAVacancyTimeout = 10;

        [Test]
        public void GetPendingQAVacanciesShouldOnlyReturnVacanciesAvailableToQa()
        {
            // Arrange
            var today = new DateTime(2016, 3, 16, 12, 0, 0);
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var dateTimeService = new Mock<IDateTimeService>();
            var vacancyLockingService = new Mock<IVacancyLockingService>();
            dateTimeService.Setup(dts => dts.UtcNow).Returns(today);
            const int anInt = 1;
            const string username = "userName";
            const int submissionCount = 2;
            const int vacancyAvailableToQAReferenceNumber = 1;
            const int vacancyNotAvailableToQAReferenceNumber = 2;

            var apprenticeshipVacancies = new List<VacancySummary>
            {
                new VacancySummary
                {
                    ClosingDate = today,
                    DateSubmitted = today,
                    OwnerPartyId = anInt,
                    VacancyReferenceNumber = vacancyAvailableToQAReferenceNumber,
                    Status = VacancyStatus.ReservedForQA,
                    QAUserName = username,
                    DateStartedToQA = null,
                    SubmissionCount = submissionCount
                },
                new VacancySummary
                {
                    ClosingDate = today,
                    DateSubmitted = today,
                    OwnerPartyId = anInt,
                    VacancyReferenceNumber = vacancyNotAvailableToQAReferenceNumber,
                    Status = VacancyStatus.ReservedForQA,
                    QAUserName = username,
                    DateStartedToQA = null,
                    SubmissionCount = submissionCount
                }
            };

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProviderViaOwnerParty(It.IsAny<int>())).Returns(new Provider());

            vacancyLockingService.Setup(
                vls =>
                    vls.IsVacancyAvailableToQABy(username,
                        It.Is<VacancySummary>(vs => vs.VacancyReferenceNumber == vacancyAvailableToQAReferenceNumber))).Returns(true);

            vacancyLockingService.Setup(
                vls =>
                    vls.IsVacancyAvailableToQABy(username,
                        It.Is<VacancySummary>(vs => vs.VacancyReferenceNumber == vacancyNotAvailableToQAReferenceNumber))).Returns(false);

            var currentUserService = new Mock<ICurrentUserService>();
            currentUserService.Setup(cus => cus.CurrentUserName).Returns(username);

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .With(dateTimeService)
                    .With(vacancyLockingService)
                    .With(currentUserService)
                    .Build();

            var vacancies = vacancyProvider.GetPendingQAVacancies();
            vacancies.Should().HaveCount(1);
            vacancies.Single().VacancyReferenceNumber.Should().Be(vacancyAvailableToQAReferenceNumber);

        }

        [Test]
        public void GetVacanciesPendingQAShouldCallRepositoryWithPendingQAAsDesiredStatus()
        {
            //Arrange
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            const int ownerPartyId = 42;
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration {BlacklistedCategoryCodes = ""});

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                .Returns(new List<VacancySummary>
                {
                    new VacancySummary
                    {
                        ClosingDate = DateTime.Now,
                        DateSubmitted = DateTime.Now,
                        OwnerPartyId = ownerPartyId,
                        Status = VacancyStatus.Submitted
                    }
                });

            providerService.Setup(ps => ps.GetProviderViaOwnerParty(ownerPartyId)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .With(configurationService)
                    .Build();

            //Act
            vacancyProvider.GetPendingQAVacancies();

            //Assert
            vacancyPostingService.Verify(avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA));
            providerService.Verify(ps => ps.GetProviderViaOwnerParty(ownerPartyId), Times.Once);
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